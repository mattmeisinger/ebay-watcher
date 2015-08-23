using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Call;
using EbayAPIHelper.Models;

namespace eBayWatcher.eBay
{
    public class EbayClientHelper
    {
        public static ApiContext GetSdkClient()
        {
            EbaySettings.Validate();

            var apiContext = new ApiContext()
            {
                SoapApiServerUrl = EbaySettings.TradingServerAddress,
                ApiCredential = new ApiCredential
                {
                    ApiAccount = new ApiAccount
                    {
                        Application = EbaySettings.AppID,
                        Developer = EbaySettings.DevID,
                        Certificate = EbaySettings.CertID
                    },
                    //eBayToken = EbaySettings.EBayToken
                },
                Site = SiteCodeType.US,
                RuName = EbaySettings.RuName
            };

            return apiContext;
        }

        public static SuggestedCategoryType[] FindCategories(string eBayToken, string searchTerm)
        {
            var client = GetSdkClient();
            client.ApiCredential.eBayToken = eBayToken;
            var req = new GetSuggestedCategoriesCall(client);
            var categories = req.GetSuggestedCategories(searchTerm).Cast<SuggestedCategoryType>().ToArray();
            return categories;
        }
    }
}
