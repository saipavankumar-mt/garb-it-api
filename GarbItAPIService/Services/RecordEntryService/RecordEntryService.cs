using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecordEntryService
{
    public class RecordEntryService : IRecordEntryService
    {
        private IRecordEntryProvider _recordEntryProvider;
        private IClientProvider _clientProvider;

        public RecordEntryService(IRecordEntryProvider recordEntryProvider, IClientProvider clientProvider)
        {
            _recordEntryProvider = recordEntryProvider;
            _clientProvider = clientProvider;
        }

        public async Task<AddRecordResponse> AddRecordEntryAsync(string qrCodeId)
        {
            //Get clientInfo from QR Code ID

            var clientInfo = await _clientProvider.GetClientInfoAsync(qrCodeId);

            //Get Employee Info from Session key

            var recordInfo = new RecordEntryInfo()
            {
                ClientId = clientInfo.Id,
                ClientName = clientInfo.Name,
                EmployeeId = AmbientContext.Current.UserInfo.Id,
                EmployeeName = AmbientContext.Current.UserInfo.Name,
                Municipality = clientInfo.Municipality,
                Location = clientInfo.Location
            };

            return await _recordEntryProvider.AddRecordEntryAsync(recordInfo);
        }

        public async Task<CountResponse> GetActiveClientsCountAsync()
        {
            var countResponse = new CountResponse();
            var recordsInfo = await SearchRecordAsync(new List<SearchRequest>(), DateTime.Today.AddMonths(-1).ToString(), DateTime.Today.ToString());

            if(recordsInfo != null)
            {
                countResponse.Count = recordsInfo.Select(x => x.ClientId).Distinct().Count();
            }

            return countResponse;
        }

        public async Task<List<EmployeeScannedCountResponse>> GetEmployeesScannedCountAsync(string fromDate, string toDate)
        {
            var recordsInfo = await SearchRecordAsync(new List<SearchRequest>(), fromDate, toDate);
            if (recordsInfo != null)
            {
                var employeeGroup = recordsInfo.GroupBy(x => x.EmployeeName).Select(x => new EmployeeScannedCountResponse() { EmployeeName = x.Key, Count = x.Count() });

                return employeeGroup?.ToList();
            }

            return new List<EmployeeScannedCountResponse>();
        }

        public async Task<CountResponse> GetScannedRecordsCountAsync(string fromDate, string toDate)
        {
            var role = AmbientContext.Current.UserInfo.Role;
            var searchRequests = new List<SearchRequest>();
            if (role.Equals(Role.SuperAdmin))
            {
                searchRequests.Add(new SearchRequest() { SearchByKey = "Location", SearchByValue = AmbientContext.Current.UserInfo.Location });
            }

            if (role.Equals(Role.Admin))
            {
                searchRequests.Add(new SearchRequest() { SearchByKey = "Municipality", SearchByValue = AmbientContext.Current.UserInfo.Municipality });
            }

            var count = await _recordEntryProvider.GetCollectedCountAsync(searchRequests, DateTime.Parse(fromDate), DateTime.Parse(toDate));
            return new CountResponse()
            {
                Count = count
            };
        }

        public async Task<List<RecordScannedDayCountResponse>> GetScannedRecordsDayCountAsync(string fromDate, string toDate)
        {
            var role = AmbientContext.Current.UserInfo.Role;
            var searchRequests = new List<SearchRequest>();
            if (role.Equals(Role.SuperAdmin))
            {
                searchRequests.Add(new SearchRequest() { SearchByKey = "Location", SearchByValue = AmbientContext.Current.UserInfo.Location });
            }

            if (role.Equals(Role.Admin))
            {
                searchRequests.Add(new SearchRequest() { SearchByKey = "Municipality", SearchByValue = AmbientContext.Current.UserInfo.Municipality });
            }

            var recordsCollected = await _recordEntryProvider.GetCollectedRecordsAsync(searchRequests, DateTime.Parse(fromDate), DateTime.Parse(toDate));

            var response = new List<RecordScannedDayCountResponse>();

            var groupedResponse = recordsCollected.GroupBy(x=> TruncateTime(x.ScannedDateTime)).Select(x => new RecordScannedDayCountResponse() { Date = x.Key, Count = x.Count() });

            return groupedResponse.ToList();
        }

        public async Task<List<RecordEntryInfo>> SearchRecordAsync(List<SearchRequest> searchRequests, string fromDate, string toDate)
        {
            if(searchRequests == null)
            {
                searchRequests = new List<SearchRequest>();
            }

            var role = AmbientContext.Current.UserInfo.Role;
            if (role.Equals(Role.SuperAdmin))
            {
                searchRequests.Add(new SearchRequest() { SearchByKey = "Location", SearchByValue = AmbientContext.Current.UserInfo.Location });
            }

            if (role.Equals(Role.Admin))
            {
                searchRequests.Add(new SearchRequest() { SearchByKey = "Municipality", SearchByValue = AmbientContext.Current.UserInfo.Municipality });
            }

            return await _recordEntryProvider.GetCollectedRecordsAsync(searchRequests, DateTime.Parse(fromDate), DateTime.Parse(toDate));
        }


        private string TruncateTime(string dateTime)
        {
            return DateTime.Parse(dateTime).ToString("yyyy/MM/dd");
        }
    }
}
