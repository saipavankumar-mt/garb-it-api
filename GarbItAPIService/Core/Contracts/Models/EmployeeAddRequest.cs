﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class EmployeeAddRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Married { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
