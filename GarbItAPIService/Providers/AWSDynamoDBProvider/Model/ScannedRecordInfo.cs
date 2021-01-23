using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class ScannedRecordInfo
    {
        public ScannedRecordInfo()
        {

        }

        [DynamoDBHashKey("RecordId")]
        public string RecordId { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }        
        public string Municipality { get; set; }
        public string Location { get; set; }

        [DynamoDBRangeKey("ScannedDateTime", StoreAsEpoch = true)]
        public string ScannedDateTime { get; set; }

        [DynamoDBProperty("ExpirationTime", StoreAsEpoch = true)]
        public DateTime ExpirationTime { get; set; }
    }
}
