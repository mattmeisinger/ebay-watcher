using eBayWatcher.Core.eBay.SearchAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eBayWatcher.WebAPI.Controllers
{
    public class EbaySearchController : ApiController
    {
        [Route("EbaySearch")]
        [HttpPost]
        public object Search([FromBody] dynamic p)
        {
            //var token = Account.FromSession((string)p.sessionId).Status.Token;
            var current = FindingClient.GetCurrentItems((string)p.searchTerm, (int)p.categoryId);
            return new
            {
                Completed = FindingClient.GetCompletedItems((string)p.searchTerm, (int)p.categoryId),
                CurrentBuyItNow = current.Where(a => a.Type != "Auction").ToArray(),
                CurrentAuction = current.Where(a => a.Type == "Auction").ToArray()
            };
        }
    }
}
