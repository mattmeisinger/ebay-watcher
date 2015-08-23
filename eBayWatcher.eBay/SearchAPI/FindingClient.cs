using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eBay.Service.Finding.Finding;
using eBay.Services;

namespace eBayWatcher.eBay.SearchAPI
{
    public class FindingClient
    {
        public static FindingServicePortTypeClient GetFindingClient()
        {
            ClientConfig config = new ClientConfig
            {
                EndPointAddress = EbaySettings.FindingServerAddress,// Initialize service end-point configration
                ApplicationId = EbaySettings.AppID // set eBay developer account AppID
            };

            return FindingServiceClientFactory.getServiceClient(config);
        }

        public static EbayItem[] GetCompletedItems(string keyword, int? categoryId)
        {
            var ret = GetFindingClient().findCompletedItems(new FindCompletedItemsRequest
            {
                keywords = keyword,
                categoryId = new[] { categoryId.ToString() }
            });

            if (ret.errorMessage != null && ret.errorMessage.Any())
                throw new Exception(string.Join(Environment.NewLine, ret.errorMessage.Select(a => a.message)));

            return ret.searchResult.item
                .Where(a => a.sellingStatus.sellingState == "EndedWithSales")
                .Select(a => new EbayItem
                {
                    Id = a.itemId,
                    Name = a.title,
                    Url = a.viewItemURL,
                    ImageUrl = a.galleryURL,
                    AuctionEndTime = a.listingInfo.endTime,
                    AuctionPrice = a.sellingStatus.convertedCurrentPrice == null ? (double?)null : a.sellingStatus.convertedCurrentPrice.Value,
                    BidCount = a.sellingStatus.bidCount,
                    BuyItNowPrice = a.listingInfo.buyItNowAvailable ? a.listingInfo.buyItNowPrice.Value : (double?)null,
                    Status = "Completed",
                    Type = a.listingInfo.listingType,
                    Details = a
                })
                .ToArray();
        }
        public static EbayItem[] GetCurrentItems(string keyword, int? categoryId)
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
                .Select(a => new EbayItem
                {
                    Id = a.itemId,
                    BuyItNowPrice = a.listingInfo.convertedBuyItNowPrice == null ? (double?)null : a.listingInfo.convertedBuyItNowPrice.Value,
                    AuctionEndTime = a.listingInfo.endTimeSpecified ? a.listingInfo.endTime : (DateTime?)null,
                    AuctionPrice = a.sellingStatus.convertedCurrentPrice == null ? (double?)null : a.sellingStatus.convertedCurrentPrice.Value,
                    BidCount = a.sellingStatus.bidCountSpecified ? a.sellingStatus.bidCount : (int?)null,
                    Name = a.title,
                    Url = a.viewItemURL,
                    ImageUrl = a.galleryURL,
                    Status = "Active",
                    Type = a.listingInfo.listingType,
                    Details = a
                })
                .ToArray();
        }
        public static CategoryListItem[] FindCategories(string eBayToken, string searchTerm)
        {
            // Get suggested categories from Ebay
            var categories = EbayClientHelper.FindCategories(eBayToken, searchTerm);

            // Convert into POCO objects
            var ret = new List<CategoryListItem>();
            foreach (var a in categories)
            {
                // Build list of parent categories
                var parents = new List<CategoryListItem>();
                for (int i = 0; i < a.Category.CategoryParentID.Count; i++)
                {
                    var category = new CategoryListItem
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
                var newItem = new CategoryListItem();
                newItem.Id = a.Category.CategoryID.ToIntOrDefault().Value;
                newItem.Name = a.Category.CategoryName;
                newItem.FullName = fullCategoryName.ToString();
                ret.Add(new CategoryListItem
                {
                    Id = a.Category.CategoryID.ToIntOrDefault().Value,
                    Name = a.Category.CategoryName,
                    FullName = fullCategoryName.ToString(),
                    Parents = parents.ToArray()
                });
            }
            return ret.ToArray();
        }
    }
}
