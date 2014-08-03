using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<WishlistItem> WishlistItems { get; set; }
    }
}