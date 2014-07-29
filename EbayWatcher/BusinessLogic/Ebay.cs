using eBay.Service.Finding.Finding;
using eBay.Services;
using EbayWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.BusinessLogic
{
    public class Ebay
    {
        private static bool apiSettingsLoaded = false;

        public static FindingServicePortTypeClient GetClient()
        {
            ClientConfig config = new ClientConfig
            {
                EndPointAddress = System.Configuration.ConfigurationManager.AppSettings["FindingServerAddress"],// Initialize service end-point configration
                ApplicationId = AppSettings.Get("AppID") // set eBay developer account AppID
            };

            return FindingServiceClientFactory.getServiceClient(config);
        }

        public static string[] GetCompletedItems(string keyword, string category)
        {
            var ret = GetClient().findCompletedItems(new FindCompletedItemsRequest
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

        public static EbayItem[] GetCurrentItems(string keyword, string category)
        {
            var ret = GetClient().findItemsAdvanced(new FindItemsAdvancedRequest
            {
                keywords = keyword,
                //categoryId = new[] { "" }
            });

            if (ret.errorMessage != null && ret.errorMessage.Any())
                throw new Exception(string.Join(Environment.NewLine, ret.errorMessage.Select(a => a.message)));

            return ret.searchResult.item
                .Select(a => new EbayItem
                {
                    Id = a.itemId,
                    BuyItNowPrice = a.listingInfo.convertedBuyItNowPrice == null ? (double?)null : a.listingInfo.convertedBuyItNowPrice.Value,
                    AuctionEndTime = a.listingInfo.endTimeSpecified ? a.listingInfo.endTime : (DateTime?)null,
                    AuctionPrice = a.sellingStatus.convertedCurrentPrice == null ? null : a.sellingStatus.convertedCurrentPrice.Value,
                    BidCount = a.sellingStatus.bidCountSpecified ? a.sellingStatus.bidCount : (int?)null
                })
                .ToArray();
        }

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

        internal static string GetLoginUrl()
        {
            // var ruName = AppSettings.Get("RuName");
            // var urlEncodedSessionID = "";
            // return string.Format("https://signin.sandbox.ebay.com/ws/eBayISAPI.dll?SignIn&runame={0}&SessID={1}", ruName, urlEncodedSessionID);
            return "";
        }
    }
}