using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class AddClientResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string QRCodeId { get; set; }
    }
}
