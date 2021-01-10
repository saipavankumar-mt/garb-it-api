using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IRecordEntryService
    {
        Task<AddRecordResponse> AddRecordEntryAsync(string qrCodeId);
        Task<List<RecordEntryInfo>> SearchRecordAsync(List<SearchRequest> searchRequests, string fromDate, string toDate);
        Task<CountResponse> GetScannedRecordsCountAsync(string fromDate, string toDate);
        Task<CountResponse> GetActiveClientsCountAsync();
        Task<List<EmployeeScannedCountResponse>> GetEmployeesScannedCountAsync(string fromDate, string toDate);
        Task<List<RecordScannedDayCountResponse>> GetScannedRecordsDayCountAsync(string fromDate, string toDate);
    }
}
