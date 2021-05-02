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
    public class CountProvider : ICountProvider
    {
        private IDataService _dataService;
        private DBSettings _settings;

        public CountProvider(IDataService dataService, IOptions<DBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }


        public async Task<CountInfo> GetCountInfoAsync(string id)
        {
            return await _dataService.GetDataById<CountInfo>(id, _settings.TableNames.CountsInfoTable);
        }

        public async Task IncrementCountAsync(string id, bool setExpiry = true)
        {
            var countInfo = await _dataService.GetDataById<CountInfo>(id, _settings.TableNames.CountsInfoTable);
            if (countInfo == null)
            {
                var dbCountInfo = new CountInfo()
                {
                    Id = id,
                    Count = 1
                };

                await _dataService.SaveDataSql(_settings.TableNames.CountsInfoTable, dbCountInfo.ToInsertSqlCmdParams());
            }
            else
            {
                countInfo.Count++;
                await _dataService.UpdateDataSql(_settings.TableNames.CountsInfoTable, id, countInfo.ToUpdateSqlCmdParams());
            }
        }
    }
}
