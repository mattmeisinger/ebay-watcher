using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Entities.Models
{
    public class WishlistItemHistoricalItem : EbayWatcher.Models.EbayCompletedItem
    {
        public int WishlistItemId { get; set; }

        public WishlistItem WishlistItem { get; set; }
    }
}