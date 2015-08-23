using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eBay.Service.Finding.Finding;

namespace eBayWatcher.Core.Models
{
    public class EBayItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public double? BuyItNowPrice { get; set; }
        public double? AuctionPrice { get; set; }
        public DateTime? AuctionEndTime { get; set; }
        public int? BidCount { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public bool Ignore { get; set; }
        public SearchItem Details { get; set; }
    }
}