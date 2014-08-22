using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string CategoryFullName { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; }
    }
}