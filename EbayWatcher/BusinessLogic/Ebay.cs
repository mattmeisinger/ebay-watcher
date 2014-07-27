using eBay.Service.Finding.Finding;
using eBay.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.BusinessLogic
{
    public class Ebay
    {
        public static FindingServicePortTypeClient GetClient()
        {
            ClientConfig config = new ClientConfig
            {
                EndPointAddress = System.Configuration.ConfigurationManager.AppSettings["FindingServerAddress"],// Initialize service end-point configration
                ApplicationId = System.Configuration.ConfigurationManager.AppSettings["AppID"] // set eBay developer account AppID
            };

            return FindingServiceClientFactory.getServiceClient(config);
        }

        public static string[] GetCompletedItems(string keyword, string category)
        {
            var completedItems = GetClient().findCompletedItems(new FindCompletedItemsRequest
            {
                keywords = keyword ?? "ipod",
                categoryId = new[] { "" },
                paginationInput = new PaginationInput
                {
                    //entriesPerPage = 10,
                    //entriesPerPageSpecified = true
                }
            });

            return completedItems.searchResult.item
                .Select(a => a.itemId)
                .ToArray();
        }
    }
}