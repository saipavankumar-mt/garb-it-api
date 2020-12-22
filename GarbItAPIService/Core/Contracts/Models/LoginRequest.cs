using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
