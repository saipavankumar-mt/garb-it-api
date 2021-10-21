using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using SQLiteDBProvider.Translator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDBProvider.Providers
{
    public class RecordEntryProvider : IRecordEntryProvider
    {
        private IDataService _dataService;
        private DBSettings _settings;

        public RecordEntryProvider(IDataService dataService, IOptions<DBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
            _dataService.SetDataBaseSource(_settings.DatabaseLocation.RecordEntryDatabase);
        }


        public async Task<AddRecordResponse> AddRecordEntryAsync(RecordEntryInfo recordInfo)
        {
            bool result = await _dataService.SaveDataSql(_settings.TableNames.RecordEntryTable, recordInfo.ToInsertSqlCmdParams());
            if (result)
            {
                return new AddRecordResponse() { RecordId = "1" };
            }
            return new AddRecordResponse();
        }

        public async Task<int> GetCollectedCountAsync(DateTime fromDateTime, DateTime toDateTime)
        {
            int finalCount = 0;
            DateTime startDate = fromDateTime.Date;
            
            var count = await _dataService.GetDataCountByDateRange(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, new List<SearchRequest>() { new SearchRequest() { SearchByKey = "Municipality", SearchByValue = AmbientContext.Current.UserInfo.Municipality } }, "RecordId");

            finalCount += count;
            

            return finalCount;
        }

        public async Task<SearchedRecordsResponse> GetCollectedRecordsAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime, int limit = 20, string paginationToken = "")
        {
            var response = new SearchedRecordsResponse();

            var totalCount = await _dataService.GetDataCountByDateRange(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests, "RecordId");

            var result = await _dataService.QueryDataByPagination<RecordEntryInfo>(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests, limit, paginationToken, "RecordId");

            if (result.Item1 != null)
            {
                foreach (var item in result.Item1)
                {
                    item.ScannedDateTime = item?.ScannedDateTime?.ConvertDate();
                }
            }
            return new SearchedRecordsResponse()
            {
                RecordEntries = result.Item1,
                PaginationToken = result.Item2,
                TotalCount = totalCount
            };
        }

        public async Task<List<RecordEntryInfo>> ExportRecordsAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime)
        {
            var response = await _dataService.ExportData<RecordEntryInfo>(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests);
            if (response != null)
            {
                foreach (var item in response)
                {
                    item.ScannedDateTime = item?.ScannedDateTime?.ConvertDate();
                }
            }
            return response;
        }

        public async Task<int> GetEmployeeCollectedCountAsync(DateTime fromDateTime, DateTime toDateTime, string employeeName)
        {
            int finalCount = 0;
            DateTime startDate = fromDateTime.Date;

            var count = await _dataService.GetDataCountByDateRange(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, new List<SearchRequest>() { new SearchRequest() { SearchByKey = "EmployeeName", SearchByValue = employeeName } }, "RecordId");

            finalCount += count;

            return finalCount;
        }
    }
}
