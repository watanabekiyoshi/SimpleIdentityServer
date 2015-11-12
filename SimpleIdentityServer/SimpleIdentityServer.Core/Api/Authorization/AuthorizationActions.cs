﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using SimpleIdentityServer.Core.Api.Authorization.Actions;
using SimpleIdentityServer.Core.Errors;
using SimpleIdentityServer.Core.Exceptions;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.Results;
using SimpleIdentityServer.Core.Validators;
using SimpleIdentityServer.Core.Models;

namespace SimpleIdentityServer.Core.Api.Authorization
{
    public interface IAuthorizationActions
    {
        ActionResult GetAuthorization(AuthorizationParameter parameter,
            IPrincipal claimsPrincipal,
            string code);
    }

    public class AuthorizationActions : IAuthorizationActions
    {
        private readonly IGetAuthorizationCodeOperation _getAuthorizationCodeOperation;

        private readonly IGetTokenViaImplicitWorkflowOperation _getTokenViaImplicitWorkflowOperation;

        private readonly IAuthorizationCodeGrantTypeParameterValidator _authorizationCodeGrantTypeParameterValidator;

        private readonly IParameterParserHelper _parameterParserHelper;
        
        public AuthorizationActions(
            IGetAuthorizationCodeOperation getAuthorizationCodeOperation,
            IGetTokenViaImplicitWorkflowOperation getTokenViaImplicitWorkflowOperation,
            IAuthorizationCodeGrantTypeParameterValidator authorizationCodeGrantTypeParameterValidator,
            IParameterParserHelper parameterParserHelper)
        {
            _getAuthorizationCodeOperation = getAuthorizationCodeOperation;
            _getTokenViaImplicitWorkflowOperation = getTokenViaImplicitWorkflowOperation;
            _authorizationCodeGrantTypeParameterValidator = authorizationCodeGrantTypeParameterValidator;
            _parameterParserHelper = parameterParserHelper;
        }

        public ActionResult GetAuthorization(AuthorizationParameter parameter,
            IPrincipal claimsPrincipal,
            string code)
        {
            _authorizationCodeGrantTypeParameterValidator.Validate(parameter);
            var responseTypes = _parameterParserHelper.ParseResponseType(parameter.ResponseType);
            var authorizationFlow = GetAuthorizationFlow(responseTypes, parameter.State);
            switch (authorizationFlow)
            {
                case AuthorizationFlow.AuthorizationCodeFlow:
                    return _getAuthorizationCodeOperation.Execute(
                        parameter,
                        claimsPrincipal,
                        code);
                    break;
                case AuthorizationFlow.ImplicitFlow:
                    return _getTokenViaImplicitWorkflowOperation.Execute(
                        parameter,
                        claimsPrincipal,
                        code);
                    break;
                case AuthorizationFlow.HybridFlow:

                    break;
            }

            return null;
        }

        private static AuthorizationFlow GetAuthorizationFlow(ICollection<ResponseType> responseTypes, string state)
        {
            var record = Constants.MappingResponseTypesToAuthorizationFlows.Keys
                .SingleOrDefault(k => k.Count == responseTypes.Count && k.All(key => responseTypes.Contains(key)));
            if (record == null)
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    ErrorDescriptions.TheAuthorizationFlowIsNotSupported,
                    state);
            }

            return Constants.MappingResponseTypesToAuthorizationFlows[record];
        }
    }
}