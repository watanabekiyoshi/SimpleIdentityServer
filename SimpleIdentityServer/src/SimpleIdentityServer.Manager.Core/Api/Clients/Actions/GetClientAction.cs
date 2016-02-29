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

using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.Manager.Core.Errors;
using SimpleIdentityServer.Manager.Core.Exceptions;
using System;

namespace SimpleIdentityServer.Manager.Core.Api.Clients.Actions
{
    public interface IGetClientAction
    {
        Client Execute(string clientId);
    }

    public class GetClientAction : IGetClientAction
    {
        private IClientRepository _clientRepository;

        #region Constructor

        public GetClientAction(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        #endregion

        #region Public methods

        public Client Execute(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var client = _clientRepository.GetClientById(clientId);
            if (client == null)
            {
                throw new IdentityServerManagerException(ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.TheClientDoesntExist, clientId));
            }

            return client;
        }

        #endregion
    }
}
