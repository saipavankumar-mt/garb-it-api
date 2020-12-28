using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class SessionResponse
    {
        public string SessionKey { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public Role Role { get; set; }
    }
}
