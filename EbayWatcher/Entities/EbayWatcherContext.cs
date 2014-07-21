using StampFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StampFinder.Entities
{
    public class EbayWatcherContext : DbContext
    {
        public DbSet<WishlistItem> Stamps { get; set; }
    }
}