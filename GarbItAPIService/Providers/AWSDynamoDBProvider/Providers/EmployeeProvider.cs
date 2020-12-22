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
        private IPasswordProvider _passwordProvider;
        private AWSDynamoDBSettings _settings;

        public EmployeeProvider(IDataService dataService, IPasswordProvider passwordProvider, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _passwordProvider = passwordProvider;
            _settings = options.Value;
        }

        public async Task<List<EmployeeInfo>> GetEmployees(string reportsToId = "")
        {
            var response = new List<EmployeeInfo>();

            if (string.IsNullOrEmpty(reportsToId))
            {
                response = await _dataService.GetData<EmployeeInfo>(_settings.TableNames.EmployeeTable);
            }
            else
            {
                response = await _dataService.GetData<EmployeeInfo>(_settings.TableNames.EmployeeTable, "ReportsToId", reportsToId);
            }

            return response;

        }

        public async Task<EmployeeInfo> GetEmployeeInfoAsync(string id)
        {
            return await _dataService.GetDataById<EmployeeInfo>(id, _settings.TableNames.EmployeeTable);
        }

        public async Task<bool> AddEmployee(EmployeeInfo employeeInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.EmployeeTable);

            var req = employeeInfo.ToDBModel(nextId);

            if (await _dataService.SaveData(req, _settings.TableNames.EmployeeTable))
            {
                var passwordEntry = GetPasswordEntry(req);
                if (await _passwordProvider.AddUserToRegistry(passwordEntry))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> RemoveEmployeeInfoByIdAsync(string id)
        {
            var employee = await GetEmployeeInfoAsync(id);

            if (employee != null)
            {
                if (await _dataService.RemoveDataByIdAsync<EmployeeInfo>(id, _settings.TableNames.EmployeeTable))
                {
                    await _passwordProvider.RemoveUserFromPasswordRegistry(employee.UserName);
                }

                return true;
            }

            return false;
        }


        private static PasswordInfo GetPasswordEntry(Model.EmployeeInfo req)
        {
            return new PasswordInfo()
            {
                Id = req.EmployeeId,
                UserName = req.UserName,
                Password = req.Password,
                Role = Role.Employee
            };
        }
    }
}
