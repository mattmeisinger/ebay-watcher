using eBayWatcher.Core.Models;
using eBayWatcher.WebAPI.Filters;
using eBayWatcher.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eBayWatcher.WebAPI.Controllers
{
    [EnsureMatchingTokenAndUsername]
    public class WatchListItemsController : ApiBaseController
    {
        [Route("WatchListItems")]
        [HttpGet]
        public WatchListItem[] Get()
        {
            var username = GetHeader("eBayWatcherUsername");
            return DynamoDB.WatchListItems.Get(username);
        }

        [Route("WatchListItems")]
        [HttpPost]
        public WatchListItem Save([FromBody] AddWatchListItemModel p)
        {
            var item = p.Item;
            item.Id = Guid.NewGuid();
            item.UserId = p.Username;
            DynamoDB.WatchListItems.Save(item);
            return item;
        }

        [Route("WatchListItems/{id}")]
        [HttpDelete]
        public void Delete(string id)
        {
            var username = GetHeader("eBayWatcherUsername");
            DynamoDB.WatchListItems.Delete(id, username);
        }
    }
}
