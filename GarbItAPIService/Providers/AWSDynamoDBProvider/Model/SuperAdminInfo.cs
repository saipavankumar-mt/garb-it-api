using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class SuperAdminInfo
    {
        [DynamoDBHashKey("SuperAdminId")]
        public string SuperAdminId { get; set; }        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Muncipality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
