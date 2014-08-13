using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Finding.Finding;
using eBay.Services;
using EbayWatcher.Entities;
using EbayWatcher.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace EbayWatcher.BusinessLogic
{
    public class Ebay
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

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
            HttpContext.Current.Session["SessionId"] = sessionId;

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
            using (var context = new EbayWatcherContext())
            {
                var client = GetSdkClient();
                var req = new GetSuggestedCategoriesCall(client);
                var categories = req.GetSuggestedCategories(searchTerm);

                // Convert into POCO objects
                var suggestedCategories = new List<SuggestedCategory>();
                foreach (var a in categories.Cast<SuggestedCategoryType>())
                {
                    // Build list of parent categories
                    var parents = new List<Entities.Models.Category>();
                    for (int i = 0; i < a.Category.CategoryParentID.Count; i++)
                    {
                        var category = new Entities.Models.Category
                        {
                            Id = a.Category.CategoryParentID[i].ToIntOrDefault().Value,
                            Name = a.Category.CategoryParentName[i]
                        };
                        parents.Add(category);
                    }

                    // Go through the categories and determine what the full category
                    // name is (with parents included in the string)
                    var fullCategoryName = new StringBuilder();
                    foreach (var item in parents)
                    {
                        fullCategoryName.Append(item.Name);
                        item.FullName = fullCategoryName.ToString();
                        fullCategoryName.Append(" > ");
                    }
                    fullCategoryName.Append(a.Category.CategoryName);

                    // Create new category
                    var newItem = new SuggestedCategory();
                    newItem.Id = a.Category.CategoryID.ToIntOrDefault().Value;
                    newItem.Name = a.Category.CategoryName;
                    newItem.Parents = parents.ToArray();
                    newItem.FullName = fullCategoryName.ToString();
                    suggestedCategories.Add(newItem);
                }

                // Add any categories that don't exist yet to the database
                var categoryIdsInDatabase = context.Categories.Select(a => a.Id).ToList();
                var ebayQueryResults = suggestedCategories.Select(a => a as Entities.Models.Category).Union(suggestedCategories.SelectMany(a => a.Parents)).ToArray();
                foreach (var item in ebayQueryResults)
                {
                    if (!categoryIdsInDatabase.Contains(item.Id))
                    {
                        context.Categories.Add(item);
                        categoryIdsInDatabase.Add(item.Id);
                    }
                }
                context.SaveChanges();

                return suggestedCategories.ToArray();
            }
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

        internal static bool StartedAuthenticatingWithEbay()
        {
            // If SessionId has already been sent, then that means the user has already been
            // sent to the login page.
            var sessionId = HttpContext.Current.Session["SessionId"].ToStringOrDefault();
            return !sessionId.IsNullOrWhiteSpace();
        }

        internal static bool IsAuthenticatedWithEbay()
        {
            if (Users.IsLoggedIn())
            {
                return true;
            }
            else
            {
                // If they started the login process, check if they completed and a token is waiting.
                if (StartedAuthenticatingWithEbay())
                {
                    // Get token from Ebay
                    var sessionId = HttpContext.Current.Session["SessionId"].ToStringOrDefault();
                    var client = GetSdkClient();
                    var call = new FetchTokenCall(client);
                    var token = call.FetchToken(sessionId);

                    // If the token comes back empty, that means they didn't complete the sign-in process.
                    if (token.IsNullOrWhiteSpace())
                    {
                        return false;
                    }
                    else
                    {
                        // Otherwise get the user id of the logged in user from Ebay
                        var userCall = new ConfirmIdentityCall(client);
                        var userId = userCall.ConfirmIdentity(sessionId).ToLowerOrDefault(); // Always use the lowercase version of the username

                        // Set session variables identifying the user
                        HttpContext.Current.Session["Token"] = token;
                        HttpContext.Current.Session["UserId"] = userId;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}