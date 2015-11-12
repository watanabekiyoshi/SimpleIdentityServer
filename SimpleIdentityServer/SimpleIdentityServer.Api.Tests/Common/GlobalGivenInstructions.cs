﻿using SimpleIdentityServer.Api.Tests.Common.Fakes.Models;
using SimpleIdentityServer.Api.Tests.Extensions;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.DataAccess.Fake;
using SimpleIdentityServer.DataAccess.Fake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SimpleIdentityServer.Api.Tests.Common
{
    [Binding]
    public class GlobalGivenInstructions
    {
        private readonly ISecurityHelper _securityHelper;
        
        public GlobalGivenInstructions()
        {
            _securityHelper = new SecurityHelper();
        }

        [BeforeScenario]
        public static void Before()
        {
            FakeDataSource.Instance().Init();
        }
        
        [Given("a mobile application (.*) is defined")]
        public void GivenClient(string clientId)
        {
            var client = new Client
            {
                ClientId = clientId,
                AllowedScopes = new List<Scope>()
            };

            FakeDataSource.Instance().Clients.Add(client);
        }

        [Given("a resource owner with username (.*) and password (.*) is defined")]
        public void GivenResourceOwner(string userName, string password)
        {
            var resourceOwner = new ResourceOwner
            {
                UserName = userName,
                Password = _securityHelper.ComputeHash(password)
            };

            FakeDataSource.Instance().ResourceOwners.Add(resourceOwner);
        }
        
        [Given("create a RSA key")]
        public void GivenCreateRsaKey()
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                var serializedRsa = provider.ToXmlString(true);
                FakeDataSource.Instance().JsonWebKeys.Add(new JsonWebKey
                {
                    Alg = AllAlg.RS256,
                    KeyOps = new[]
                        {
                        KeyOperations.Sign
                    },
                    Kid = "1",
                    Kty = KeyType.RSA,
                    SerializedKey = serializedRsa
                });
            }
        }

        [Given("the scopes are defined")]
        public void GivenScope(Table table)
        {
            var scopes = table.CreateSet<FakeScope>();
            foreach (var scope in scopes)
            {
                FakeDataSource.Instance().Scopes.Add(scope.ToFake());
            }
        }
        
        [Given("the scopes (.*) are assigned to the client (.*)")]
        public void GivenScopesToTheClients(List<string> scopeNames, string clientId)
        {
            var client = GetClient(clientId);
            if (client == null)
            {
                return;
            }

            var scopes = FakeDataSource.Instance().Scopes;
            foreach (var scopeName in scopeNames)
            {
                var storedScope = scopes.SingleOrDefault(s => s.Name == scopeName);
                if (storedScope == null)
                {
                    continue;
                }

                client.AllowedScopes.Add(storedScope);
            }
        }

        [Given("the id_token signature algorithm is set to (.*) for the client (.*)")]
        public void GivenIdTokenSignatureAlgorithmIsSetForTheClient(string algorithm, string clientId)
        {
            var client = GetClient(clientId);
            if (client == null)
            {
                return;
            }

            client.IdTokenSignedTResponseAlg = algorithm;
        }

        [Given("the grant-type (.*) is supported by the client (.*)")]
        public void GivenGrantTypesAreSupportedByClient(GrantType grantType, string clientId)
        {
            var client = GetClient(clientId);
            if (client == null)
            {
                return;
            }

            client.GrantTypes = new List<GrantType>
            {
                grantType
            };
        }

        [Given("the response-types (.*) are supported by the client (.*)")]
        public void GivenResponseIsSupportedByTheClient(List<string> responseTypes, string clientId)
        {
            var client = GetClient(clientId);
            if (client == null)
            {
                return;
            }

            client.ResponseTypes = new List<ResponseType>();
            foreach (var responseType in responseTypes)
            {
                var resp = (ResponseType)Enum.Parse(typeof(ResponseType), responseType);
                client.ResponseTypes.Add(resp);
            }
        }

        [Given("the consent has been given by the resource owner (.*) for the client (.*) and scopes (.*)")]
        public void GivenConsent(string resourceOwnerId, string clientId, List<string> scopeNames)
        {
            var client = FakeDataSource.Instance().Clients.SingleOrDefault(c => c.ClientId == clientId);
            var resourceOwner = FakeDataSource.Instance().ResourceOwners.SingleOrDefault(r => r.Id == resourceOwnerId);
            var scopes = new List<Scope>();
            foreach (var scopeName in scopeNames)
            {
                var storedScope = FakeDataSource.Instance().Scopes.SingleOrDefault(s => s.Name == scopeName);
                scopes.Add(storedScope);
            }
            var consent = new Consent
            {
                Client = client,
                GrantedScopes = scopes,
                ResourceOwner = resourceOwner
            };

            FakeDataSource.Instance().Consents.Add(consent);
        }

        private static Client GetClient(string clientId)
        {
            var client = FakeDataSource.Instance().Clients.SingleOrDefault(c => c.ClientId == clientId);
            return client;
        }
    }
}