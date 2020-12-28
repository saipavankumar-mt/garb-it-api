using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class EmployeeInfo : UserInfo
    {
        public EmployeeInfo()
        {

        }

        public string ReportsToId { get; set; }
        public string ReportsToName { get; set; }
    }
}
