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

using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.Manager.Core.Parameters;
using System;

namespace SimpleIdentityServer.Manager.Core.Api.Configuration.Actions
{
    public interface IUpdateConfigurationAction
    {
        bool Execute(UpdateConfigurationParameter updateConfigurationParameter);
    }

    internal class UpdateConfigurationAction : IUpdateConfigurationAction
    {
        #region Fields

        private readonly IConfigurationRepository _configurationRepository;

        #endregion

        #region Constructor

        public UpdateConfigurationAction(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        #endregion

        #region Public methods

        public bool Execute(UpdateConfigurationParameter updateConfigurationParameter)
        {
            if (updateConfigurationParameter == null)
            {
                throw new ArgumentNullException(nameof(updateConfigurationParameter));
            }

            if (string.IsNullOrWhiteSpace(updateConfigurationParameter.Key))
            {
                throw new ArgumentNullException(nameof(updateConfigurationParameter.Key));
            }

            return _configurationRepository.Update(new SimpleIdentityServer.Core.Models.Configuration
            {
                Key = updateConfigurationParameter.Key,
                Value = updateConfigurationParameter.Value
            });
        }

        #endregion
    }
}