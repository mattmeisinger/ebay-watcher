using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Call;
using EbayAPIHelper.Models;

namespace EbayAPIHelper
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
                    eBayToken = EbaySettings.EBayToken
                },
                Site = SiteCodeType.US,
                RuName = EbaySettings.RuName
            };

            return apiContext;
        }

        public static SuggestedCategoryType[] FindCategories(string searchTerm)
        {
            var client = GetSdkClient();
            var req = new GetSuggestedCategoriesCall(client);
            var categories = req.GetSuggestedCategories(searchTerm).Cast<SuggestedCategoryType>().ToArray();
            return categories;
        }
    }
}
