using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class SearchedRecordsResponse
    {
        public SearchedRecordsResponse()
        {
            RecordEntries = new List<RecordEntryInfo>();
        }

        public List<RecordEntryInfo> RecordEntries { get; set; }
        public string PaginationToken { get; set; }
        public int TotalCount { get; set; }
    }
}
