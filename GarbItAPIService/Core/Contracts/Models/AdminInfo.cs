﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class AdminInfo : UserInfo
    {
        public string AdminId { get; set; }        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ReportsToId { get; set; }
        public string ReportsToName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Muncipality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        
    }
}
