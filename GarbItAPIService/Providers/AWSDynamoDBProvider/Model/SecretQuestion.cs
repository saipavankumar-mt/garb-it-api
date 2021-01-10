using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class SecretQuestion
    {
        [DynamoDBHashKey("Id")]
        public string Id { get; set; }

        public string Question { get; set; }
    }
}
