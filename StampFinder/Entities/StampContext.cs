using StampFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StampFinder.Entities
{
    public class StampContext : DbContext
    {
        public DbSet<Stamp> Stamps { get; set; }
    }
}