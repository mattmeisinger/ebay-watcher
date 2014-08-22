using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Models
{
    public class CategoryListItem
    {

        public Entities.Category Item { get; set; }

        public Entities.Category[] Parents { get; set; }
    }
}