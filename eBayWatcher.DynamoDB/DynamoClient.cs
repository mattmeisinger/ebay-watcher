using Amazon.DynamoDBv2;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBayWatcher.DynamoDB
{
    class DynamoClient
    {
        public static AmazonDynamoDBClient Create()
        {
            //var config = new AmazonDynamoDBConfig();
            //config.ServiceURL = "http://dynamodb.us-west-2.amazonaws.com";

            var accessKeyId = getEnvVariable("AWS_ACCESS_KEY_ID");
            var secretAccessKey = getEnvVariable("AWS_SECRET_ACCESS_KEY");
            var awsDefaultRegion = getEnvVariable("AWS_DEFAULT_REGION");
            var region = Amazon.RegionEndpoint.EnumerableAllRegions.Where(a => a.SystemName == awsDefaultRegion).Single();

            var client = new AmazonDynamoDBClient(accessKeyId, secretAccessKey, region);

            return client;
        }

        private static string getEnvVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"Environment variable {name} not found. This variable is required.");
            return value;
        }
    }
}
