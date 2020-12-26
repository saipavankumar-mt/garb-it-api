using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class RecordEntryInfo
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Location { get; set; }
        public string ScannedDateTime { get; set; }
    }
}
