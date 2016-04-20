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

using SimpleIdentityServer.Uma.Core.Errors;
using SimpleIdentityServer.Uma.Core.Exceptions;
using SimpleIdentityServer.Uma.Core.Helpers;
using SimpleIdentityServer.Uma.Core.Parameters;
using SimpleIdentityServer.Uma.Core.Repositories;
using System;
using System.Linq;

namespace SimpleIdentityServer.Uma.Core.Api.PolicyController.Actions
{
    public interface IAddAuthorizationPolicyAction
    {

    }

    internal class AddAuthorizationPolicyAction
    {
        private readonly IPolicyRepository _policyRepository;

        private readonly IResourceSetRepository _resourceSetRepository;

        private readonly IRepositoryExceptionHelper _repositoryExceptionHelper;

        #region Constructor

        public AddAuthorizationPolicyAction(
            IPolicyRepository policyRepository,
            IResourceSetRepository resourceSetRepository,
            IRepositoryExceptionHelper repositoryExceptionHelper)
        {
            _policyRepository = policyRepository;
            _resourceSetRepository = resourceSetRepository;
            _repositoryExceptionHelper = repositoryExceptionHelper;
        }

        #endregion

        #region Public methods

        public string Execute(AddPolicyParameter addPolicyParameter)
        {
            if (addPolicyParameter == null)
            {
                throw new ArgumentNullException(nameof(addPolicyParameter));
            }

            if (string.IsNullOrWhiteSpace(addPolicyParameter.ResourceSetId))
            {
                throw new BaseUmaException(ErrorCodes.InvalidRequestCode,
                        string.Format(ErrorDescriptions.TheParameterNeedsToBeSpecified, "resource_set_id"));
            }

            if (addPolicyParameter.IsCustom)
            {
                if (string.IsNullOrWhiteSpace(addPolicyParameter.Script))
                {
                    throw new BaseUmaException(ErrorCodes.InvalidRequestCode,
                        string.Format(ErrorDescriptions.TheParameterNeedsToBeSpecified, "script"));
                }
            }
            else
            {
                if (addPolicyParameter.Scopes == null ||
                    !addPolicyParameter.Scopes.Any())
                {
                    throw new BaseUmaException(ErrorCodes.InvalidRequestCode,
                        string.Format(ErrorDescriptions.TheParameterNeedsToBeSpecified, "scopes"));
                }

                if (addPolicyParameter.ClientIdsAllowed == null ||
                    !addPolicyParameter.ClientIdsAllowed.Any())
                {
                    throw new BaseUmaException(ErrorCodes.InvalidRequestCode,
                        string.Format(ErrorDescriptions.TheParameterNeedsToBeSpecified, "allowed_clients"));
                }
            }


            var resourceSet = _repositoryExceptionHelper.HandleException(
                string.Format(ErrorDescriptions.TheResourceSetCannotBeRetrieved, addPolicyParameter.ResourceSetId),
                () => _resourceSetRepository.GetResourceSetById(addPolicyParameter.ResourceSetId));
            if (resourceSet == null)
            {
                throw new BaseUmaException(ErrorCodes.InvalidResourceSetId,
                    string.Format(ErrorDescriptions.TheResourceSetDoesntExist, addPolicyParameter.ResourceSetId));
            }

            return string.Empty;
        }

        #endregion
    }
}
