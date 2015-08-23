using Amazon.DynamoDBv2.DocumentModel;
using eBayWatcher.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace eBayWatcher.DynamoDB
{
    public class Users
    {
        public static void AddUserToken(string username, string token)
        {
            using (var client = DynamoClient.Create())
            {
                var tableName = ConfigurationManager.AppSettings["DynamoDBUsersTableARN"].Split('/').Last();
                var table = Table.LoadTable(client, tableName);
                var dbItem = table.GetItem(username);
                if (dbItem == null)
                {
                    // Create a new user object
                    dbItem = new Document();
                    dbItem["UserId"] = username;
                    dbItem["Tokens"] = new List<string> { token };
                }
                else
                {
                    // Add token to the array
                    var tokens = dbItem["Tokens"].AsArrayOfString().ToList();
                    tokens.Add(token);
                    dbItem["Tokens"] = tokens.Distinct().ToList();
                }
                table.UpdateItem(dbItem);
            }
        }

        public static User Get(string username)
        {
            using (var client = DynamoClient.Create())
            {
                var tableName = ConfigurationManager.AppSettings["DynamoDBUsersTableARN"].Split('/').Last();
                var table = Table.LoadTable(client, tableName);
                var dbItem = table.GetItem(username);
                if (dbItem == null)
                    return null;

                return new User
                {
                    Id = dbItem["UserId"].AsString(),
                    Tokens = dbItem["Tokens"].AsArrayOfString()
                };
            }
        }
    }
}
