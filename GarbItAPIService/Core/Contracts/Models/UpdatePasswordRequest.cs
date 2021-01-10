using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class UpdatePasswordRequest
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
    }
}
