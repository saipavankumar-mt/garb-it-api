using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class CountInfo
    {
        [DynamoDBHashKey("Id")]
        public string Id { get; set; }
        public int Count { get; set; }

        [DynamoDBProperty("ExpirationTime", StoreAsEpoch = true)]
        public DateTime ExpirationTime { get; set; }
    }
}
