using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class AWSDynamoDBSettings
    {
        public TableNames TableNames { get; set; }
        public NextIdGeneratorValue NextIdGeneratorValue { get; set; }
    }

    public class TableNames
    {
        public string SuperAdminTable { get; set; }
        public string AdminTable { get; set; }
        public string EmployeeTable { get; set; }
        public string IdGeneratorTable { get; set; }
        public string PasswordRegistryTable { get; set; }
        public string SessionTable { get; set; }
        public string ClientTable { get; set; }
        public string RecordEntryTable { get; set; }
    }

    public class NextIdGeneratorValue
    {
        public int SuperAdmin { get; set; }
        public int Admin { get; set; }
        public int Employee { get; set; }
        public int Client { get; set; }
        public int Record { get; set; }

    }
}
