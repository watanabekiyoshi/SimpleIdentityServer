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
using SimpleIdentityServer.Manager.Core.Api.Scopes.Actions;
using System.Collections.Generic;

namespace SimpleIdentityServer.Manager.Core.Api.Scopes
{
    public interface IScopeActions
    {
        bool DeleteScope(string scopeName);

        Scope GetScope(string scopeName);

        List<Scope> GetScopes();

        bool AddScope(Scope scope);
    }

    internal class ScopeActions : IScopeActions
    {
        private readonly IDeleteScopeOperation _deleteScopeOperation;

        private readonly IGetScopeOperation _getScopeOperation;

        private readonly IGetScopesOperation _getScopesOperation;

        private readonly IAddScopeOperation _addScopeOperation;

        #region Constructor

        public ScopeActions(
            IDeleteScopeOperation deleteScopeOperation,
            IGetScopeOperation getScopeOperation,
            IGetScopesOperation getScopesOperation,
            IAddScopeOperation addScopeOperation)
        {
            _deleteScopeOperation = deleteScopeOperation;
            _getScopeOperation = getScopeOperation;
            _getScopesOperation = getScopesOperation;
            _addScopeOperation = addScopeOperation;
        }

        #endregion

        #region Public methods

        public bool DeleteScope(string scopeName)
        {
            return _deleteScopeOperation.Execute(scopeName);
        }

        public Scope GetScope(string scopeName)
        {
            return _getScopeOperation.Execute(scopeName);
        }

        public List<Scope> GetScopes()
        {
            return _getScopesOperation.Execute();
        }

        public bool AddScope(Scope scope)
        {
            return _addScopeOperation.Execute(scope);
        }

        #endregion
    }
}
