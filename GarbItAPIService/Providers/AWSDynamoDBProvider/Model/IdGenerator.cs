using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    [DynamoDBTable("IdGenerator")]
    public class IdGenerator
    {
        [DynamoDBHashKey("Table")]
        public string Table { get; set; }
        
        public int NextId { get; set; }
    }
}
