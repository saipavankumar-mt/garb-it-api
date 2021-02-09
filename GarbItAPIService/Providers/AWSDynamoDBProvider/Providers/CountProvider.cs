using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class CountProvider : ICountProvider
    {
        private IDataService _dataService;
        private AWSDynamoDBSettings _settings;

        public CountProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }


        public async Task<CountInfo> GetCountInfoAsync(string id)
        {
            return await _dataService.GetDataById<CountInfo>(id, _settings.TableNames.CountsInfoTable);
        }

        public async Task IncrementCountAsync(string id)
        {
            var countInfo = await _dataService.GetDataById<CountInfo>(id, _settings.TableNames.CountsInfoTable);
            if (countInfo == null)
            {
                countInfo = new CountInfo()
                {
                    Id = id,
                    Count = 1
                };
            }
            else
            {
                countInfo.Count++;
            }
            
            await _dataService.UpdateData(countInfo, _settings.TableNames.CountsInfoTable);
        }
    }
}
