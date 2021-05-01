using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class DBSettings
    {
        public TableNames TableNames { get; set; }
        public NextIdGeneratorValue NextIdGeneratorValue { get; set; }
        public UserIdPrefix UserIdPrefix { get; set; }
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
        public string SecretQuestionsTable { get; set; }
        public string CountsInfoTable { get; set; }
    }

    public class NextIdGeneratorValue
    {
        public int SuperAdmin { get; set; }
        public int Admin { get; set; }
        public int Employee { get; set; }
        public int Client { get; set; }
        public int Record { get; set; }
        public int SecretQuestion { get; set; }

    }

    public class UserIdPrefix
    {
        public string SuperAdmin { get; set; }
        public string Admin { get; set; }
        public string Employee { get; set; }
        public string Client { get; set; }
        public string Record { get; set; }
        public string SecretQuestion { get; set; }
    }
}
