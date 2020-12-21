using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class EmployeeProvider : IEmployeeProvider
    {
        private IDataService _dataService;
        private AWSDynamoDBSettings _settings;

        public EmployeeProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }

        public async Task<List<EmployeeInfo>> GetEmployees()
        {
            var response = await _dataService.GetData<EmployeeInfo>(_settings.TableNames.EmployeeTable);
            return response;

        }

        public async Task<bool> AddEmployee(EmployeeInfo employeeInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.EmployeeTable);

            var req = employeeInfo.ToDBModel(nextId);

            return await _dataService.SaveData(req, _settings.TableNames.EmployeeTable);
        }
    }
}
