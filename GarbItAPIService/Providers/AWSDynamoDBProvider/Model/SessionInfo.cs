using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class SessionInfo
    {
        [DynamoDBHashKey("SessionId")]
        public string SessionId { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        [DynamoDBProperty("ExpiryTime", true)]
        public long ExpiryTime { get; set; }
    }
}
