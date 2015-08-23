using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eBayWatcher.Core.Models;
using Amazon.DynamoDBv2.DocumentModel;
using System.Configuration;

namespace eBayWatcher.DynamoDB
{
    public class WatchListItems
    {
        public static WatchListItem[] Get(string username)
        {
            using (var client = DynamoClient.Create())
            {
                var tableName = ConfigurationManager.AppSettings["DynamoDBWatchListItemsTableARN"].Split('/').Last();
                var table = Table.LoadTable(client, tableName);
                var query = table.Query(username, new QueryFilter());
                var results = query.GetRemaining();
                return results
                    .Select(a => new WatchListItem
                    {
                        UserId = a["UserId"].AsString(),
                        Id = a["ItemId"].AsGuid(),
                        Name = a["Name"].AsString(),
                        SearchText = a["SearchText"].AsString(),
                        CategoryId = a["CategoryId"].AsInt(),
                        CategoryName = a["CategoryName"].AsString()
                    })
                    .ToArray();
            }
        }

        public static void Add(WatchListItem item)
        {
            using (var client = DynamoClient.Create())
            {
                var tableName = ConfigurationManager.AppSettings["DynamoDBWatchListItemsTableARN"].Split('/').Last();
                var table = Table.LoadTable(client, tableName);
                var dbItem = new Document();
                dbItem["UserId"] = item.UserId;
                dbItem["ItemId"] = item.Id;
                dbItem["Name"] = item.Name;
                dbItem["SearchText"] = item.SearchText;
                dbItem["CategoryId"] = item.CategoryId;
                dbItem["CategoryName"] = item.CategoryName;
                table.UpdateItem(dbItem);
            }
        }
    }
}
