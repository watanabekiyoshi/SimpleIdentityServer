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

using SimpleIdentityServer.Core.Jwt;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Core.Repositories
{
    public interface IJsonWebKeyRepository
    {
        Task<ICollection<JsonWebKey>> GetAllAsync();
        Task<ICollection<JsonWebKey>> GetByAlgorithmAsync(Use use, AllAlg algorithm, KeyOperations[] operations);
        Task<JsonWebKey> GetByKidAsync(string kid);
        Task<bool> DeleteAsync(JsonWebKey jsonWebKey);
        Task<bool> InsertAsync(JsonWebKey jsonWebKey);
        Task<bool> UpdateAsync(JsonWebKey jsonWebKey);
    }
}
