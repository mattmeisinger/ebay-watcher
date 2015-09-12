using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBayWatcher.DynamoDB
{
    public static class DynamoDBExtensionMethods
    {
        public static string AsStringOrDefault(this DynamoDBEntry o)
        {
            if (o == null)
                return null;
            else
                return o.AsString();
        }

        public static int? AsIntOrDefault(this DynamoDBEntry o)
        {
            if (o == null)
                return null;
            else
                return o.AsInt();
        }

        public static string[] AsArrayOfStringOrDefault(this DynamoDBEntry o)
        {
            if (o == null)
                return new string[] { };
            else
                return o.AsArrayOfString();
        }

        public static bool? AsBooleanOrDefault(this DynamoDBEntry o)
        {
            if (o == null)
                return null;
            else
                return o.AsBoolean();
        }

        public static string GetString(this Document d, string key)
        {
            if (d.ContainsKey(key))
                return d[key].AsString();
            else
                return null;
        }
        public static Guid? GetGuid(this Document d, string key)
        {
            if (d.ContainsKey(key))
                return d[key].AsGuid();
            else
                return null;
        }
        public static int? GetInt(this Document d, string key)
        {
            if (d.ContainsKey(key))
                return d[key].AsInt();
            else
                return null;
        }
        public static string[] GetArrayOfString(this Document d, string key)
        {
            if (d.ContainsKey(key))
                return d[key].AsArrayOfString();
            else
                return new string[] { };
        }
    }
}
