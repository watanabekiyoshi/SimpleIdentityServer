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

namespace SimpleIdentityServer.Authentication.Middleware
{
    public class Constants
    {
        public static class IdentityProviderNames
        {
            public const string Microsoft = "Microsoft";
            public const string Facebook = "Facebook";
            public const string Google = "Google";
            public const string Adfs = "ADFS";
            public const string Cookies = "Cookies";
            public const string Eid = "EID";
            public const string Twitter = "Twitter";
            public const string Github = "Github";
        }

        public static string CookieName = "SimpleIdServer-external";
    }
}
