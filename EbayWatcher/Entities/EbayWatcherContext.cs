using EbayWatcher.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EbayWatcher.Entities
{
    public class EbayWatcherContext : DbContext
    {
        public DbSet<WishlistItem> WishlistItems { get; set; }
    }
}