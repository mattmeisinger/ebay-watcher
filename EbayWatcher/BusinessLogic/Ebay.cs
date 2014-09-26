using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Finding.Finding;
using eBay.Services;
using EbayWatcher.Entities;
using EbayWatcher.Models;
using System;
using System.Collections.Generic;
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
                ApiCredential = new ApiCredential
                {
                    ApiAccount = new ApiAccount
                    {
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
        internal static string GetNewSessionId()
        {
            var ruName = AppSettings.Get("RuName");
            var client = GetSdkClient();
            var call = new GetSessionIDCall(client);
            var sessionId = call.GetSessionID(ruName);
            return sessionId;
        }
        internal static string GetLoginUrl(string sessionId)
        {
            var ruName = AppSettings.Get("RuName");
            var urlEncodedSessionID = HttpUtility.UrlEncode(sessionId);
            return string.Format("https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame={0}&SessID={1}", ruName, urlEncodedSessionID);
        }
        #endregion

        #region Data Calls
        public static EbayCompletedItem[] GetCompletedItems(string keyword, int categoryId)
        {
            var ret = GetFindingClient().findCompletedItems(new FindCompletedItemsRequest
            {
                keywords = keyword,
                categoryId = new[] { categoryId.ToString() }
            });

            if (ret.errorMessage != null && ret.errorMessage.Any())
                throw new Exception(string.Join(Environment.NewLine, ret.errorMessage.Select(a => a.message)));

            return ret.searchResult.item
                .Select(a => new EbayCompletedItem
                {
                    Id = a.itemId,
                    Name = a.title,
                    Url = a.viewItemURL,
                    ImageUrl = a.galleryURL,
                    AuctionEndTime = a.listingInfo.endTime,
                    AuctionPrice = a.sellingStatus.convertedCurrentPrice == null ? (double?)null : a.sellingStatus.convertedCurrentPrice.Value,
                    BidCount = a.sellingStatus.bidCount,
                    BuyItNowPrice = a.listingInfo.buyItNowAvailable ? a.listingInfo.buyItNowPrice.Value : (double?)null,
                })
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
        internal static CategoryListItem[] FindCategories(string searchTerm)
        {
            // Get suggested categories from Ebay
            using (var context = new EbayWatcherContext())
            {
                var client = GetSdkClient();
                var req = new GetSuggestedCategoriesCall(client);
                var categories = req.GetSuggestedCategories(searchTerm);

                // Convert into POCO objects
                var ret = new List<CategoryListItem>();
                foreach (var a in categories.Cast<SuggestedCategoryType>())
                {
                    // Build list of parent categories
                    var parents = new List<Entities.Category>();
                    for (int i = 0; i < a.Category.CategoryParentID.Count; i++)
                    {
                        var category = new Entities.Category
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
                    var newItem = new Entities.Category();
                    newItem.Id = a.Category.CategoryID.ToIntOrDefault().Value;
                    newItem.Name = a.Category.CategoryName;
                    newItem.FullName = fullCategoryName.ToString();
                    ret.Add(new CategoryListItem
                    {
                        Item = new Entities.Category
                        {
                            Id = a.Category.CategoryID.ToIntOrDefault().Value,
                            Name = a.Category.CategoryName,
                            FullName = fullCategoryName.ToString()
                        },
                        Parents = parents.ToArray()
                    });
                }

                // Add any categories that don't exist yet to the database
                var categoryIdsInDatabase = context.Categories.Select(a => a.Id).ToList();
                var allQueryCategoryResults = ret.Select(a => a.Item).Union(ret.SelectMany(a => a.Parents)).ToArray();
                foreach (var item in allQueryCategoryResults)
                {
                    if (!categoryIdsInDatabase.Contains(item.Id))
                    {
                        context.Categories.Add(item);
                        categoryIdsInDatabase.Add(item.Id);
                    }
                }
                context.SaveChanges();
                return ret.ToArray();
            }
        }
        internal static bool CompleteEbayAuthentication()
        {
            using (var context = new EbayWatcherContext())
            {
                // Get token from Ebay
                var sessionId = Users.GetCurrentSessionId();
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
                    var ebayUsername = userCall.ConfirmIdentity(sessionId).ToLowerOrDefault(); // Always use the lowercase version of the username

                    // Create user if it doesn't already exist
                    var user = context.Users.SingleOrDefault(a => a.EbayUsername == ebayUsername);
                    if (user == null)
                    {
                        user = new User()
                        {
                            EbayUsername = ebayUsername
                        };
                        context.Users.Add(user);
                    }

                    user.EbaySessionId = sessionId;
                    user.EbayToken = token;

                    if (context.GetValidationErrors().Any())
                        throw new Exception(string.Join(Environment.NewLine, context.GetValidationErrors().SelectMany(a => a.ValidationErrors).Select(a => a.PropertyName + ": " + a.ErrorMessage)));

                    context.SaveChanges();

                    HttpContext.Current.Session["EbaySessionId"] = sessionId;
                    HttpContext.Current.Session["EbayToken"] = token;
                    HttpContext.Current.Session["EbayUsername"] = ebayUsername;

                    return true;
                }
            }
        }
        #endregion
    }
}