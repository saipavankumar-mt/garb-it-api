using AWSDynamoDBProvider.Model;
using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class RecordEntryProvider : IRecordEntryProvider
    {
        private IDataService _dataService;
        private AWSDynamoDBSettings _settings;
        private ICountProvider _countProvider;

        public RecordEntryProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options, ICountProvider countProvider)
        {
            _dataService = dataService;
            _settings = options.Value;
            _countProvider = countProvider;
        }


        public async Task<AddRecordResponse> AddRecordEntryAsync(RecordEntryInfo recordInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.RecordEntryTable, _settings.UserIdPrefix.Record, _settings.NextIdGeneratorValue.Record, "D8");

            var req = recordInfo.ToDBModel(nextId);
            if (await _dataService.SaveData<ScannedRecordInfo>(req, _settings.TableNames.RecordEntryTable))
            {
                string counterId = String.Format("{0}-{1}-Records", DateTime.Today.ToString("yyyy-MM-dd"), AmbientContext.Current.UserInfo.Municipality);

                await _countProvider.IncrementCountAsync(counterId);

                return new AddRecordResponse()
                {
                    RecordId = req.RecordId
                };
            }

            return new AddRecordResponse();
        }



        public async Task<int> GetCollectedCountAsync(DateTime fromDateTime, DateTime toDateTime)
        {
            int finalCount = 0;
            DateTime startDate = fromDateTime.Date;

            while (startDate <= toDateTime.Date)
            {
                string counterId = String.Format("{0}-{1}-Records", fromDateTime.Date.ToString("yyyy-MM-dd"), AmbientContext.Current.UserInfo.Municipality);

                var countInfo = await _countProvider.GetCountInfoAsync(counterId);
                
                if(countInfo!=null)
                {
                    finalCount += countInfo.Count;
                }

                startDate = startDate.Date.AddDays(1);
            }

            return finalCount;
        }

        public async Task<SearchedRecordsResponse> GetCollectedRecordsAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime, int limit = 20, string paginationToken = "")
        {
            var response = new SearchedRecordsResponse();

            var totalCount = await _dataService.GetDataCountByDateRange(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests);

            var result = await _dataService.QueryDataByPagination<RecordEntryInfo>(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests, limit, paginationToken);


            return new SearchedRecordsResponse()
            {
                RecordEntries = result.Item1,
                PaginationToken = result.Item2,
                TotalCount = totalCount
            };
        }

        public async Task<List<RecordEntryInfo>> ExportRecordsAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime)
        {
            return await _dataService.ExportData<RecordEntryInfo>(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests);
        }
    }
}
