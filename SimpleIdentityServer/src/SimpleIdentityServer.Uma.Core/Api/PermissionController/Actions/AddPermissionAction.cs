﻿#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Newtonsoft.Json;
using SimpleIdentityServer.Uma.Core.Errors;
using SimpleIdentityServer.Uma.Core.Exceptions;
using SimpleIdentityServer.Uma.Core.Helpers;
using SimpleIdentityServer.Uma.Core.Models;
using SimpleIdentityServer.Uma.Core.Parameters;
using SimpleIdentityServer.Uma.Core.Repositories;
using SimpleIdentityServer.Uma.Core.Services;
using SimpleIdentityServer.Uma.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Uma.Core.Api.PermissionController.Actions
{
    internal interface IAddPermissionAction
    {
        Task<string> Execute(string clientId, AddPermissionParameter addPermissionParameters);
        Task<IEnumerable<string>> Execute(string clientId, IEnumerable<AddPermissionParameter> addPermissionParameters);
    }

    internal class AddPermissionAction : IAddPermissionAction
    {
        private readonly IResourceSetRepository _resourceSetRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IRepositoryExceptionHelper _repositoryExceptionHelper;
        private readonly IConfigurationService _configurationService;
        private readonly IUmaServerEventSource _umaServerEventSource;

        public AddPermissionAction(
            IResourceSetRepository resourceSetRepository,
            ITicketRepository ticketRepository,
            IRepositoryExceptionHelper repositoryExceptionHelper,
            IConfigurationService configurationService,
            IUmaServerEventSource umaServerEventSource)
        {
            _resourceSetRepository = resourceSetRepository;
            _ticketRepository = ticketRepository;
            _repositoryExceptionHelper = repositoryExceptionHelper;
            _configurationService = configurationService;
            _umaServerEventSource = umaServerEventSource;
        }

        public async Task<string> Execute(string clientId, AddPermissionParameter addPermissionParameter)
        {
            var result = await Execute(clientId, new[] { addPermissionParameter });
            return result.First();
        }

        public async Task<IEnumerable<string>> Execute(string clientId, IEnumerable<AddPermissionParameter> addPermissionParameters)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(clientId);
            }

            if (addPermissionParameters == null)
            {
                throw new ArgumentNullException(nameof(addPermissionParameters));
            }

            var json = addPermissionParameters == null ? string.Empty : JsonConvert.SerializeObject(addPermissionParameters);
            _umaServerEventSource.StartAddPermission(json);

            await CheckAddPermissionParameter(addPermissionParameters);
            var ticketLifetimeInSeconds = await _configurationService.GetTicketLifeTime();
            var tickets = new List<Ticket>();
            foreach(var addPermissionParameter in addPermissionParameters)
            {
                tickets.Add(new Ticket
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = clientId,
                    ExpirationDateTime = DateTime.UtcNow.AddSeconds(ticketLifetimeInSeconds),
                    Scopes = addPermissionParameter.Scopes,
                    ResourceSetId = addPermissionParameter.ResourceSetId,
                    CreateDateTime = DateTime.UtcNow
                });
            }
            
            await _repositoryExceptionHelper.HandleException(
                ErrorDescriptions.AtLeastOneTicketCannotBeInserted,
                () => _ticketRepository.Insert(tickets));
            _umaServerEventSource.FinishAddPermission(json);
            return tickets.Select(t => t.Id);
        }

        private async Task CheckAddPermissionParameter(IEnumerable<AddPermissionParameter> addPermissionParameters)
        {
            // 1. Get resource sets.
            var resourceSets = await _repositoryExceptionHelper.HandleException(
                ErrorDescriptions.TheResourceSetsCannotBeRetrieved,
                () => _resourceSetRepository.Get(addPermissionParameters.Select(p => p.ResourceSetId)));

            // 2. Check parameters & scope exist.
            foreach (var addPermissionParameter in addPermissionParameters)
            {
                if (string.IsNullOrWhiteSpace(addPermissionParameter.ResourceSetId))
                {
                    throw new BaseUmaException(
                        ErrorCodes.InvalidRequestCode,
                        string.Format(ErrorDescriptions.TheParameterNeedsToBeSpecified, Constants.AddPermissionNames.ResourceSetId));
                }

                if (addPermissionParameter.Scopes == null ||
                    !addPermissionParameter.Scopes.Any())
                {
                    throw new BaseUmaException(
                        ErrorCodes.InvalidRequestCode,
                        string.Format(ErrorDescriptions.TheParameterNeedsToBeSpecified, Constants.AddPermissionNames.Scopes));
                }

                var resourceSet = resourceSets.FirstOrDefault(r => addPermissionParameter.ResourceSetId == r.Id);
                if (resourceSet == null)
                {
                    throw new BaseUmaException(
                        ErrorCodes.InvalidResourceSetId,
                        string.Format(ErrorDescriptions.TheResourceSetDoesntExist, addPermissionParameter.ResourceSetId));
                }

                if (resourceSet.Scopes == null ||
                    addPermissionParameter.Scopes.Any(s => !resourceSet.Scopes.Contains(s)))
                {
                    throw new BaseUmaException(
                        ErrorCodes.InvalidScope,
                        ErrorDescriptions.TheScopeAreNotValid);
                }
            }
        }
    }
}
