﻿using Contracts;
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
        private IEmployeeProvider _employeeProvider;

        public RecordEntryService(IRecordEntryProvider recordEntryProvider, IClientProvider clientProvider, IEmployeeProvider employeeProvider)
        {
            _recordEntryProvider = recordEntryProvider;
            _clientProvider = clientProvider;
            _employeeProvider = employeeProvider;
        }

        public async Task<AddRecordResponse> AddRecordEntryAsync(string qrCodeId)
        {
            //Get clientInfo from QR Code ID

            var clientInfo = await _clientProvider.GetClientInfoAsync(qrCodeId);

            //Check if record already scanned for today

            var record = await ExportRecordsAsync(new List<SearchRequest>() { new SearchRequest() { SearchByKey = "ClientId", SearchByValue = clientInfo.Id } }, DateTime.Today.ToString(), DateTime.Now.ToString());

            if (record != null && record.Count > 0)
            {
                throw new Exception("Record already exist. Client is already scanned for today");
            }

            //Add record to table
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
            var recordsInfo = await ExportRecordsAsync(new List<SearchRequest>(), DateTime.Today.AddMonths(-1).ToString(), DateTime.Today.ToString());

            if (recordsInfo != null)
            {
                countResponse.Count = recordsInfo.Select(x => x.ClientId).Distinct().Count();
            }

            return countResponse;
        }

        public async Task<List<EmployeeScannedCountResponse>> GetEmployeesScannedCountAsync(string fromDate, string toDate)
        {
            var response = new List<EmployeeScannedCountResponse>();
            var employees = await _employeeProvider.GetEmployees(AmbientContext.Current.UserInfo.Id);

            foreach (var employee in employees)
            {
                response.Add(new EmployeeScannedCountResponse()
                {
                    EmployeeName = employee.Name,
                    Count = await _recordEntryProvider.GetEmployeeCollectedCountAsync(DateTime.Parse(fromDate), DateTime.Parse(toDate), employee.Name)
                });
            }

            return response;
        }

        public async Task<CountResponse> GetScannedRecordsCountAsync(string fromDate, string toDate)
        {
            var role = AmbientContext.Current.UserInfo.Role;

            var count = await _recordEntryProvider.GetCollectedCountAsync(DateTime.Parse(fromDate), DateTime.Parse(toDate));
            return new CountResponse()
            {
                Count = count
            };
        }

        public async Task<List<RecordScannedDayCountResponse>> GetScannedRecordsDayCountAsync(string fromDate, string toDate)
        {
            var startDate = DateTime.Parse(fromDate);
            var endDate = DateTime.Parse(toDate);

            var response = new List<RecordScannedDayCountResponse>();

            while (startDate <= endDate)
            {
                response.Add(new RecordScannedDayCountResponse()
                {
                    Date = startDate.ToString("yyyy-MM-dd"),
                    Count = await _recordEntryProvider.GetCollectedCountAsync(startDate, startDate)
                });

                startDate = startDate.AddDays(1);
            }

            return response;
        }

        public async Task<SearchedRecordsResponse> SearchRecordAsync(List<SearchRequest> searchRequests, string fromDate, string toDate, int limit = 20, string paginationToken = "")
        {
            if (searchRequests == null)
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

            return await _recordEntryProvider.GetCollectedRecordsAsync(searchRequests, DateTime.Parse(fromDate), DateTime.Parse(toDate), limit, paginationToken);
        }


        public async Task<List<RecordEntryInfo>> ExportRecordsAsync(List<SearchRequest> searchRequests, string fromDate, string toDate)
        {
            if (searchRequests == null)
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

            return await _recordEntryProvider.ExportRecordsAsync(searchRequests, DateTime.Parse(fromDate), DateTime.Parse(toDate));
        }

        private string TruncateTime(string dateTime)
        {
            return DateTime.Parse(dateTime).ToString("yyyy/MM/dd");
        }
    }
}
