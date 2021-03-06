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

using SimpleIdentityServer.Uma.Core.Api.CodeSampleController.Actions;
using System.IO;

namespace SimpleIdentityServer.Uma.Core.Api.CodeSampleController
{
    public interface ICodeSampleActions
    {
        MemoryStream GetBackendCode(string languageCode);

        MemoryStream GetFrontendCode(string languageCode);
    }

    internal class CodeSampleActions : ICodeSampleActions
    {
        private readonly IGetBackendCodeAction _getBackendCodeAction;
        private readonly IGetFrontendCodeAction _getFrontendCodeAction;

        public CodeSampleActions(
            IGetBackendCodeAction getBackendCodeAction,
            IGetFrontendCodeAction getFrontendCodeAction)
        {
            _getBackendCodeAction = getBackendCodeAction;
            _getFrontendCodeAction = getFrontendCodeAction;
        }

        public MemoryStream GetBackendCode(string languageCode)
        {
            return _getBackendCodeAction.Execute(languageCode);
        }

        public MemoryStream GetFrontendCode(string languageCode)
        {
            return _getFrontendCodeAction.Execute(languageCode);
        }
    }
}
