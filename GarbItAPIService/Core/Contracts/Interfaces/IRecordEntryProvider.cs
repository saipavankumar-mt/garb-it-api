using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IRecordEntryProvider
    {
        Task<AddRecordResponse> AddRecordEntryAsync(RecordEntryInfo recordInfo);

        Task<int> GetCollectedCountAsync(DateTime fromDateTime, DateTime toDateTime);

        Task<SearchedRecordsResponse> GetCollectedRecordsAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime, int limit = 20, string paginationToken = "");

        Task<List<RecordEntryInfo>> ExportRecordsAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime);
    }
}
