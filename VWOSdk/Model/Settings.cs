#pragma warning disable 1587
/**
 * Copyright 2019-2021 Wingify Software Pvt. Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#pragma warning restore 1587

using Newtonsoft.Json;
using System.Collections.Generic;

namespace VWOSdk
{
    public class Settings
    {
        [JsonConstructor]
        internal Settings(string sdkKey, List<Campaign> campaigns, int accountId, int version)
        {
            this.SdkKey = sdkKey;
            this.Campaigns = campaigns;
            this.AccountId = accountId;
            this.Version = version;
        }

        public string SdkKey { get; internal set; }
        public IReadOnlyList<Campaign> Campaigns { get; internal set; }
        public int AccountId { get; internal set; }
        public int Version { get; internal set; }
    }
}
