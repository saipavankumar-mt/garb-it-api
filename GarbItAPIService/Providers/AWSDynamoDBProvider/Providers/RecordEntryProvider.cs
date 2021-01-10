﻿using AWSDynamoDBProvider.Model;
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

        public RecordEntryProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }


        public async Task<AddRecordResponse> AddRecordEntryAsync(RecordEntryInfo recordInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.RecordEntryTable, _settings.UserIdPrefix.Record, _settings.NextIdGeneratorValue.Record, "D8");

            var req = recordInfo.ToDBModel(nextId);
            if (await _dataService.SaveData<ScannedRecordInfo>(req, _settings.TableNames.RecordEntryTable))
            {
                return new AddRecordResponse()
                {
                    RecordId = req.RecordId
                };
            }

            return new AddRecordResponse();
        }

        public async Task<int> GetCollectedCountAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime)
        {
            return await _dataService.GetDataCountByDateRange(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests);
        }

        public async Task<List<RecordEntryInfo>> GetCollectedRecordsAsync(List<SearchRequest> searchRequests, DateTime fromDateTime, DateTime toDateTime)
        {
            return await _dataService.SearchData<RecordEntryInfo>(_settings.TableNames.RecordEntryTable, "ScannedDateTime", fromDateTime, toDateTime, searchRequests);
        }
    }
}
