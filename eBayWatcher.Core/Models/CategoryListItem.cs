using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBayWatcher.Core.Models
{
    public class CategoryListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public CategoryListItem[] Parents { get; set; }
    }
}