using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class SearchClientsResponse
    {
        public SearchClientsResponse()
        {
            ClientInfos = new List<ClientInfo>();   
        }

        public List<ClientInfo> ClientInfos { get; set; }
        public string PaginationToken { get; set; }
        public int TotalCount { get; set; }
    }
}
