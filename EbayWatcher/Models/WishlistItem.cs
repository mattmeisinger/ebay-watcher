using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StampFinder.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }   
}