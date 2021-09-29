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
using System.Collections.Generic;
using System;

namespace VWOSdk
{
    internal class CampaignHelper
    {
        public static string getBucketingSeed(string userId, Campaign campaign, int? groupId)
        {
            if (groupId != null)
            {
                return groupId + "_" + userId;
            }

            if (campaign != null && campaign.IsBucketingSeedEnabled)
            {
                return campaign.Id + "_" + userId;
            }
            else
            {
                return userId;
            }
        }
        /*
        * Check if the campaign is a part of mutually exclusive group.
        *
        * @param settings      - Settings instance
        * @param campaignId    - campaign id
        * @return group id and name for the campaign.
        */
        public static Dictionary<string, dynamic> isPartOfGroup(Settings settings, int campaignId)
        {
            Dictionary<string, dynamic> groupDetails = new Dictionary<string, dynamic>();
            if (settings.CampaignGroups != null && settings.CampaignGroups.ContainsKey(campaignId.ToString()))
            {
                settings.getCampaignGroups().TryGetValue(campaignId.ToString(), out dynamic groupId);
                settings.getGroups().TryGetValue(groupId.ToString(), out Groups group);
                groupDetails["groupId"] = groupId;
                groupDetails["groupName"] = group.Name;
            }
            return groupDetails;
        }
        /*
         * Get the list of campaigns on the basis of their id.
         *
         * @param settings    - Settings instance  
         * @param groupId     - group id
         * @return list of campaigns
         */
        public static List<BucketedCampaign> getGroupCampaigns(AccountSettings settings, int groupId)
        {
            List<BucketedCampaign> campaignList = new List<BucketedCampaign>();
            if (settings.getGroups().ContainsKey(groupId.ToString()))
            {
                settings.getGroups().TryGetValue(groupId.ToString(), out Groups group);
                foreach (int campaignId in group.Campaigns)
                {
                    BucketedCampaign campaign = getCampaignBasedOnId(settings, campaignId);
                    if (campaign != null)
                    {
                        campaignList.Add(campaign);
                    }
                }
            }
            return campaignList;
        }
        /*
         * Get the campaign on the basis of campaign id.
         *
         * @param settings      - Settings instance
         * @param campaignId    - Campaign id
         * @return Campaign object.
         */
        private static BucketedCampaign getCampaignBasedOnId(AccountSettings settings, int campaignId)
        {
            BucketedCampaign campaign = null;
            foreach (BucketedCampaign eachCampaign in settings.Campaigns)
            {
                if (eachCampaign.Id == campaignId)
                {
                    campaign = eachCampaign;
                    break;
                }
            }
            return campaign;
        }

        /*
        * Get the winning campaign on the basis of bucketing value.
        *
        * @param itemList      - Shortlisted Campaigns
        * @param hashValue    - bucketing value
        * @return BucketedCampaign object.
        */
        public static BucketedCampaign getAllocatedItem(List<BucketedCampaign> itemList, double hashValue)
        {
            foreach (BucketedCampaign item in itemList)
            {
                if (hashValue >= item.StartRange && hashValue <= item.EndRange)
                {
                    return item;
                }
            }
            return null;
        }
        /*
        * Get campaign bucketing range.
        *
        * @param weight      - Weight of the campaign
        * @return integer value.
        */
        public static int GetCampaignBucketingRange(double weight)
        {
            if (weight == 0)
            {
                return 0;
            }
            double startRange = Convert.ToInt32(Math.Ceiling(weight * 100));
            return Convert.ToInt32(Math.Min(startRange, 10000));
        }
    }
}
