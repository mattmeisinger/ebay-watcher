using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Entities.Models
{
    public class WishlistItemIgnoreItem
    {
        public int Id { get; set; }
        public int WishlistItemId { get; set; }
        public int EbayId { get; set; }

        public WishlistItem WishlistItem { get; set; }
    }
}