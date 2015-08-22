using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBayWatcher.WebAPI.Models
{
    public class EbayCompletedItem : EbayItem
    {
        public string ImageUrl { get; set; }
    }
}