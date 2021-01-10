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

        public async Task<int> GetEmployeeCountAsync(string reportsToId = "")
        {
            if (string.IsNullOrEmpty(reportsToId))
            {
                return await _dataService.GetDataCount(_settings.TableNames.EmployeeTable);
            }
            else
            {
                return await _dataService.GetDataCount(_settings.TableNames.EmployeeTable, "ReportsToId", reportsToId);
            }
        }

        public async Task<List<EmployeeInfo>> SearchEmployeesAsync(List<SearchRequest> searchRequests)
        {
            var response = new List<EmployeeInfo>();

            response = await _dataService.SearchData<EmployeeInfo>(_settings.TableNames.EmployeeTable, searchRequests);

            return response;
        }

        public async Task<EmployeeInfo> GetEmployeeInfoAsync(string id)
        {
            return await _dataService.GetDataById<EmployeeInfo>(id, _settings.TableNames.EmployeeTable);
        }

        

        public async Task<AddUserResponse> AddEmployee(EmployeeInfo employeeInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.EmployeeTable, _settings.UserIdPrefix.Employee, _settings.NextIdGeneratorValue.Employee);

            var req = employeeInfo.ToDBModel(nextId);

            if (await _dataService.SaveData(req, _settings.TableNames.EmployeeTable))
            {
                var passwordEntry = GetPasswordEntry(req);
                if (await _passwordProvider.AddUserToRegistry(passwordEntry))
                {
                    return new AddUserResponse() { 
                        Id = req.Id,
                        Name = req.Name
                    };
                }
            }

            return new AddUserResponse();
        }

        public async Task<AddUserResponse> UpdateEmployeeAsync(EmployeeInfo employeeInfo)
        {
            var req = employeeInfo.ToDBModel();

            if (await _dataService.UpdateData(req, _settings.TableNames.EmployeeTable))
            {
                return new AddUserResponse()
                {
                    Id = req.Id,
                    Name = req.Name
                };
            }

            return new AddUserResponse();
        }

        public async Task<RemoveUserResponse> RemoveEmployeeInfoByIdAsync(string id)
        {
            var employee = await GetEmployeeInfoAsync(id);

            if (employee != null)
            {
                if (await _dataService.RemoveDataByIdAsync<EmployeeInfo>(id, _settings.TableNames.EmployeeTable))
                {
                    await _passwordProvider.RemoveUserFromPasswordRegistry(employee.UserName);
                }

                return new RemoveUserResponse()
                {
                    Id = employee.Id,
                    Name = employee.Name
                };
            }

            return new RemoveUserResponse();
        }

        public async Task<SuccessResponse> UpdateEmployeePasswordAsync(UpdatePasswordRequest req)
        {
            var userInfo = await GetEmployeeInfoAsync(req.Id);
            userInfo.Password = req.NewPassword;

            userInfo.UpdatedById = AmbientContext.Current.UserInfo.Id;
            userInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;
            userInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            var passwordInfo = await _passwordProvider.GetUserPassword(userInfo.UserName);
            passwordInfo.Password = req.NewPassword;

            await UpdateEmployeeAsync(userInfo);
            var response = await _passwordProvider.UpdatePasswordAsync(passwordInfo);
            return new SuccessResponse() { Success = true };
        }

        private static PasswordInfo GetPasswordEntry(Model.EmployeeInfo req)
        {
            return new PasswordInfo()
            {
                Id = req.Id,
                UserName = req.UserName,
                Password = req.Password,
                Role = Role.Employee,
                Name = req.Name
            };
        }

        
    }
}
