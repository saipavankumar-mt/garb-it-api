﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class ClientInfo : UserInfo
    {
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Muncipality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string QRCodeId { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
    }
}
