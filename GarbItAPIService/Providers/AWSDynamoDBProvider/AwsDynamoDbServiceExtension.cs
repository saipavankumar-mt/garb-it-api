using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWSDynamoDBProvider
{
    public static class AwsDynamoDbServiceExtension
    {
        public static void AwsDynamoDbServiceSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
        }


        public static string GetStringValue(this Document item, string key)
        {
            if (item != null && item.Keys.Contains(key))
            {
                return item[key];
            }
            return null;
        }

        public static int GetIntValue(this Document item, string key)
        {
            var intVal = 0;
            if (item != null && item.Keys.Contains(key))
            {
                int.TryParse(item[key], out intVal);
            }
            return intVal;
        }
    }
}
