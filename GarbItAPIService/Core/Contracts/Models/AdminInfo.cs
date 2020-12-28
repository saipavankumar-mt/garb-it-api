using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class AdminInfo : UserInfo
    {
        public string ReportsToId { get; set; }
        public string ReportsToName { get; set; }
    }
}
