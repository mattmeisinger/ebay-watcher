using eBayWatcher.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBayWatcher.WebAPI.Models
{
    public class AddWatchListItemModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public WatchListItem Item { get; set; }
    }
}