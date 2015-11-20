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
            if (username.IsNullOrWhiteSpace())
                return new WatchListItem[] { };

            using (var client = DynamoClient.Create())
            {
                var tableName = ConfigurationManager.AppSettings["DynamoDBWatchListItemsTableARN"].Split('/').Last();
                var table = Table.LoadTable(client, tableName);
                var query = table.Query(username, new QueryFilter());
                var results = query.GetRemaining();

                return results
                    .Where(a => !a.ContainsKey("IsDeleted") || a["IsDeleted"].AsBoolean() == false)
                    .Select(a => new WatchListItem
                    {
                        UserId = a.GetString("UserId"),
                        Id = a.GetGuid("ItemId").Value,
                        Name = a.GetString("Name"),
                        SearchText = a.GetString("SearchText"),
                        CategoryId = a.GetInt("CategoryId"),
                        CategoryName = a.GetString("CategoryName"),
                        IgnoredItemIds = a.GetArrayOfString("IgnoredItemIds").Select(b => b.ToIntOrDefault()).Where(b => b.HasValue).Select(b => b.Value).ToList(),
                        PinnedItemIds = a.GetArrayOfString("PinnedItemIds").Select(b => b.ToIntOrDefault()).Where(b => b.HasValue).Select(b => b.Value).ToList()
                    })
                    .ToArray();
            }
        }

        public static void Save(WatchListItem item)
        {
            using (var client = DynamoClient.Create())
            {
                var tableName = ConfigurationManager.AppSettings["DynamoDBWatchListItemsTableARN"].Split('/').Last();
                var table = Table.LoadTable(client, tableName);
                var dbItem = new Document();

                // Key
                dbItem["UserId"] = item.UserId;
                dbItem["ItemId"] = item.Id;

                // Fields to update
                dbItem["Name"] = item.Name;
                dbItem["SearchText"] = item.SearchText;
                dbItem["CategoryId"] = item.CategoryId;
                dbItem["CategoryName"] = item.CategoryName;
                dbItem["IgnoredItemIds"] = item.IgnoredItemIds.Select(a => a.ToString()).ToArray();
                dbItem["PinnedItemIds"] = item.PinnedItemIds.Select(a => a.ToString()).ToArray();

                table.UpdateItem(dbItem);

                // TODO: Check to make sure the item is not deleted before updating it.
            }
        }

        /// <summary>
        /// Deleting items is performed by setting the IsDeleted property. No records should ever be fully removed.
        /// </summary>
        /// <param name="id">Watch list item ID.</param>
        /// <param name="username"></param>
        public static void Delete(string id, string username)
        {
            using (var client = DynamoClient.Create())
            {
                var tableName = ConfigurationManager.AppSettings["DynamoDBWatchListItemsTableARN"].Split('/').Last();
                var table = Table.LoadTable(client, tableName);
                var dbItem = new Document();
                
                // Key
                dbItem["UserId"] = username;
                dbItem["ItemId"] = id;

                // Fields to update
                dbItem["IsDeleted"] = true;

                table.UpdateItem(dbItem);
            }
        }
    }
}
