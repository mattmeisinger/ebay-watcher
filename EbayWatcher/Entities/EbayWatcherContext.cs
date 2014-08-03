using EbayWatcher.Entities.Models;
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
        public DbSet<WishlistItemHistoricalItem> WishlistItemHistoricalItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WishlistItemIgnoreItem> WishlistItemIgnoreItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WishlistItem>().HasRequired(p => p.Category)
                .WithMany(a => a.WishlistItems)
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Entity<WishlistItemHistoricalItem>().HasRequired(p => p.WishlistItem)
                .WithMany(a => a.WishlistItemHistoricalItems)
                .HasForeignKey(a => a.WishlistItemId);

            modelBuilder.Entity<WishlistItemIgnoreItem>().HasRequired(p => p.WishlistItem)
                .WithMany(a => a.WishlistItemIgnoreItems)
                .HasForeignKey(a => a.WishlistItemId);
        }
    }
}