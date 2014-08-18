using EbayWatcher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Models
{
    public class SuggestedCategory : Category
    {
        public Category[] Parents { get; set; }
    }
}