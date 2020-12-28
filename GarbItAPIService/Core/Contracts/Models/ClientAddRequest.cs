using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class ClientAddRequest
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
