using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Finding.Finding;
using eBay.Services;
using EbayWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EbayWatcher.Utilities;

namespace EbayWatcher.BusinessLogic
{
    public class Ebay
    {
        #region Client Setup
        private static bool apiSettingsLoaded = false;
        public static bool ApiSettingsValid()
        {
            if (apiSettingsLoaded)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(AppSettings.Get("DevID")) || string.IsNullOrWhiteSpace(AppSettings.Get("AppID")))
            {
                return false;
            }
            else
            {
                apiSettingsLoaded = true;
                return true;
            }
        }
        public static FindingServicePortTypeClient GetFindingClient()
        {
            ClientConfig config = new ClientConfig
            {
                EndPointAddress = System.Configuration.ConfigurationManager.AppSettings["FindingServerAddress"],// Initialize service end-point configration
                ApplicationId = AppSettings.Get("AppID") // set eBay developer account AppID
            };

            return FindingServiceClientFactory.getServiceClient(config);
        }
        public static ApiContext GetSdkClient()
        {
            var apiContext = new ApiContext()
            {
                SoapApiServerUrl = System.Configuration.ConfigurationManager.AppSettings["TradingServerAddress"],
                ApiCredential = new ApiCredential {
                    ApiAccount = new ApiAccount {
                        Application = AppSettings.Get("AppID"),
                        Developer = AppSettings.Get("DevID"),
                        Certificate = AppSettings.Get("CertID")
                    },
                    eBayToken = AppSettings.Get("EBayToken")
                },
                Site = eBay.Service.Core.Soap.SiteCodeType.US
            };

            return apiContext;
        }
        internal static string GetLoginUrl()
        {
            var ruName = AppSettings.Get("RuName");
            var client = GetSdkClient();
            var call = new GetSessionIDCall(client);
            var sessionId = call.GetSessionID(ruName);
            var urlEncodedSessionID = HttpUtility.UrlEncode(sessionId);
            return string.Format("https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame={0}&SessID={1}", ruName, urlEncodedSessionID);
        }
        #endregion

        public static string[] GetCompletedItems(string keyword, string category)
        {
            var ret = GetFindingClient().findCompletedItems(new FindCompletedItemsRequest
            {
                keywords = keyword,
                //categoryId = new[] { "" }
            });

            if (ret.errorMessage.Any())
                throw new Exception(string.Join(Environment.NewLine, ret.errorMessage.Select(a => a.message)));

            return ret.searchResult.item
                .Select(a => a.itemId)
                .ToArray();
        }

        public static EbayCurrentItem[] GetCurrentItems(string keyword, int categoryId)
        {
            var context = GetFindingClient();
            var ret = context.findItemsAdvanced(new FindItemsAdvancedRequest
            {
                keywords = keyword,
                categoryId = new[] { categoryId.ToString() }
            });

            if (ret.errorMessage != null && ret.errorMessage.Any())
                throw new Exception(string.Join(Environment.NewLine, ret.errorMessage.Select(a => a.message)));

            return ret.searchResult.item
                .Select(a => new EbayCurrentItem
                {
                    Id = a.itemId,
                    BuyItNowPrice = a.listingInfo.convertedBuyItNowPrice == null ? (double?)null : a.listingInfo.convertedBuyItNowPrice.Value,
                    AuctionEndTime = a.listingInfo.endTimeSpecified ? a.listingInfo.endTime : (DateTime?)null,
                    AuctionPrice = a.sellingStatus.convertedCurrentPrice == null ? (double?)null : a.sellingStatus.convertedCurrentPrice.Value,
                    BidCount = a.sellingStatus.bidCountSpecified ? a.sellingStatus.bidCount : (int?)null
                })
                .ToArray();
        }

        internal static SuggestedCategory[] FindCategories(string searchTerm)
        {
            // Get suggested categories from Ebay
            var context = GetSdkClient();
            var req = new GetSuggestedCategoriesCall(context);
            var categories = req.GetSuggestedCategories(searchTerm);

            // Convert into POCO objects
            return categories.Cast<SuggestedCategoryType>()
                .Select(a => new SuggestedCategory
                {
                    Id = a.Category.CategoryID.ToIntOrDefault().Value,
                    Name = a.Category.CategoryName,
                    Parents = a.Category.CategoryParentID.Cast<string>()
                        .Select((id, index) => new Entities.Models.Category
                        {
                            Id = a.Category.CategoryParentID[index].ToIntOrDefault().Value,
                            Name = a.Category.CategoryParentName[index]
                        })
                        .ToArray()
                })
                .ToArray();
        }

        //internal static void GetCategories()
        //{
        //    var context = GetSdkClient();
        //    var req = new GetCategoriesCall(context);
        //    req.ViewAllNodes = true;
        //    req.DetailLevelList.Add(eBay.Service.Core.Soap.DetailLevelCodeType.ReturnAll);
        //    var categories = req.GetCategories();
        //    foreach (CategoryType item in categories)
        //    {
        //        System.Diagnostics.Debug.WriteLine(item);
        //    }
        //}
    }
}