using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class ClientInfo : DBBase
    {
        public ClientInfo()
        {

        }

        [DynamoDBHashKey("QRCodeId")]
        public string QRCodeId { get; set; }
        public string Id { get; set; }        
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Married { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }        
    }
}
