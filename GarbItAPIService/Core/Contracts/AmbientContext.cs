using Contracts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Contracts
{
    public class AmbientContext : IDisposable
    {
        private static readonly AsyncLocal<AmbientContext> ScopeStack = new AsyncLocal<AmbientContext>();

        public string ApiSessionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }


        public AmbientContext(AmbientContext ambientContext)
        {
            AmbientContext newContext = null;

            if (ambientContext != null)
            {
                newContext = JsonConvert.DeserializeObject<AmbientContext>(JsonConvert.SerializeObject(ambientContext));
            }

            ScopeStack.Value = newContext;
        }

        public AmbientContext(string sessionId)
        {
            ApiSessionId = sessionId;
            ScopeStack.Value = this;
        }


        [JsonConstructor]
        private AmbientContext()
        {
        }

        public static AmbientContext Current
        {
            get => ScopeStack.Value;
            private set => ScopeStack.Value = value;
        }

        public void Dispose()
        {
            if (ScopeStack.Value != null)
            {
                ScopeStack.Value = null;
            }
        }
    }
}
