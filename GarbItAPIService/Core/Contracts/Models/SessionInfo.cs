using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class SessionInfo
    {
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string Municipality { get; set; }
        public Role Role { get; set; }
    }
}
