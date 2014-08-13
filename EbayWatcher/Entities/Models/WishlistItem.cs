using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Entities.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public Category Category { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<WishlistItemHistoricalItem> WishlistItemHistoricalItems { get; set; }
        public ICollection<WishlistItemIgnoreItem> WishlistItemIgnoreItems { get; set; }
    }   
}