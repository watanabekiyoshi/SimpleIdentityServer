﻿#region copyright
// Copyright 2017 Habart Thierry
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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.EventStore.Host.DTOs.Responses
{
    [DataContract]
    public class SearchResultResponse
    {
        [DataMember(Name = Constants.SearchResultResponseNames.TotalResult)]
        public int TotalResult { get; set; }
        [DataMember(Name = Constants.SearchResultResponseNames.ItemsPerPage)]
        public int ItemsPerPage { get; set; }
        [DataMember(Name = Constants.SearchResultResponseNames.StartIndex)]
        public int StartIndex { get; set; }
        [DataMember(Name = Constants.SearchResultResponseNames.Resources)]
        public IEnumerable<object> Resources { get; set; }
    }
}
