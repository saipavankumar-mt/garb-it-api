using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    [DynamoDBTable("AdminInfo")]
    public class AdminInfo
    {
        [DynamoDBHashKey("AdminId")]
        public string AdminId { get; set; }

        public string AdminName { get; set; }
    }
}
