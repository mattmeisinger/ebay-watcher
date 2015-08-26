using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBayWatcher.Core.Models
{
    public class WatchListItem
    {
        public WatchListItem()
        {
            IgnoredEbayItemIds = new List<int>();
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string SearchText { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<int> IgnoredEbayItemIds { get; set; }

        public bool IsDeleted { get; set; }
    }
}
