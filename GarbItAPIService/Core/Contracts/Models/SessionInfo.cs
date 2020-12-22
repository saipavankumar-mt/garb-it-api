using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class SessionInfo
    {
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}
