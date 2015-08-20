using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Finding.Finding;
using eBay.Services;
using EbayAPIHelper;
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
        public static FindingServicePortTypeClient GetFindingClient()
        {
            ClientConfig config = new ClientConfig
            {
                EndPointAddress = EbaySettings.FindingServerAddress,// Initialize service end-point configration
                ApplicationId = EbaySettings.AppID // set eBay developer account AppID
            };

            return FindingServiceClientFactory.getServiceClient(config);
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
        public static CategoryListItem[] FindCategories(string searchTerm)
        {
            // Get suggested categories from Ebay
            using (var context = new EbayWatcherContext())
            {
                var categories = EbayClientHelper.FindCategories(searchTerm);

                // Convert into POCO objects
                var ret = new List<CategoryListItem>();
                foreach (var a in categories)
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
        public static bool CompleteEbayAuthentication()
        {
            var sessionId = Users.GetCurrentSessionId();
            var authorization = EbayAuth.CompleteEbayAuthentication(sessionId);
            if (authorization == null)
                return false; // The authorization hasn't gone through yet

            using (var context = new EbayWatcherContext())
            {
                // Save user credentials in user record
                var user = context.Users.SingleOrDefault(a => a.EbayUsername == authorization.EbayUsername);
                if (user == null)
                {
                    user = new User()
                    {
                        EbayUsername = authorization.EbayUsername
                    };
                    context.Users.Add(user);
                }
                user.EbaySessionId = sessionId;
                user.EbayToken = authorization.Token;
                if (context.GetValidationErrors().Any())
                    throw new Exception(string.Join(Environment.NewLine, context.GetValidationErrors().SelectMany(a => a.ValidationErrors).Select(a => a.PropertyName + ": " + a.ErrorMessage)));
                context.SaveChanges();

                // Put credentials into the current session
                HttpContext.Current.Session["EbaySessionId"] = sessionId;
                HttpContext.Current.Session["EbayToken"] = authorization.Token;
                HttpContext.Current.Session["EbayUsername"] = authorization.EbayUsername;

                return true;
            }
        }
        #endregion
    }
}