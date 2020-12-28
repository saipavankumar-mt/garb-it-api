using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class PasswordRegistry
    {
        [DynamoDBHashKey("UserName")]
        public string UserName { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
