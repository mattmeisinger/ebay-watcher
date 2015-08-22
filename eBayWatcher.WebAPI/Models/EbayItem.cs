using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBayWatcher.WebAPI.Models
{
    public class EbayItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public double? BuyItNowPrice { get; set; }
        public double? AuctionPrice { get; set; }
        public DateTime? AuctionEndTime { get; set; }
        public int? BidCount { get; set; }
    }
}