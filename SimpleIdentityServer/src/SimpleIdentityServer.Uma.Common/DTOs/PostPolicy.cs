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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Uma.Common.DTOs
{
    [DataContract]
    public class PostClaim
    {
        [DataMember(Name = ClaimNames.Type)]
        public string Type { get; set; }
        [DataMember(Name = ClaimNames.Value)]
        public string Value { get; set; }
    }

    [DataContract]
    public class PostPolicyRule
    {
        [DataMember(Name = PolicyRuleNames.ClientIdsAllowed)]
        public List<string> ClientIdsAllowed { get; set; }
        [DataMember(Name = PolicyRuleNames.Scopes)]
        public List<string> Scopes { get; set; }
        [DataMember(Name = PolicyRuleNames.Claims)]
        public List<PostClaim> Claims { get; set; }
        [DataMember(Name = PolicyRuleNames.IsResourceOwnerConsentNeeded)]
        public bool IsResourceOwnerConsentNeeded { get; set; }
        [DataMember(Name = PolicyRuleNames.Script)]
        public string Script { get; set; }
    }

    [DataContract]
    public class PostPolicy
    {
        [DataMember(Name = PolicyNames.ResourceSetIds)]
        public List<string> ResourceSetIds { get; set; }
        [DataMember(Name = PolicyNames.Rules)]
        public List<PostPolicyRule> Rules { get; set; }
    }
}
