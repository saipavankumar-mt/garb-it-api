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
    public class EmployeeProvider : IEmployeeProvider
    {
        private IDataService _dataService;
        private IPasswordProvider _passwordProvider;
        private DBSettings _settings;

        public EmployeeProvider(IDataService dataService, IPasswordProvider passwordProvider, IOptions<DBSettings> options)
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
            if (response != null)
            {
                foreach (var item in response)
                {
                    item.DateOfBirth = item?.DateOfBirth?.ConvertDate();
                    item.CreatedDateTime = item?.CreatedDateTime?.ConvertDate();
                    item.UpdatedDateTime = item?.UpdatedDateTime?.ConvertDate();
                }
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
            if (response != null)
            {
                foreach (var item in response)
                {
                    item.DateOfBirth = item?.DateOfBirth?.ConvertDate();
                    item.CreatedDateTime = item?.CreatedDateTime?.ConvertDate();
                    item.UpdatedDateTime = item?.UpdatedDateTime?.ConvertDate();
                }
            }
            return response;
        }

        public async Task<EmployeeInfo> GetEmployeeInfoAsync(string id)
        {
            var response = await _dataService.GetDataById<EmployeeInfo>(id, _settings.TableNames.EmployeeTable);
            if (response != null)
            {
                response.DateOfBirth = response?.DateOfBirth?.ConvertDate();
                response.CreatedDateTime = response?.CreatedDateTime?.ConvertDate();
                response.UpdatedDateTime = response?.UpdatedDateTime?.ConvertDate();
            }
            return response;
        }

        

        public async Task<AddUserResponse> AddEmployee(EmployeeInfo employeeInfo)
        {
            if (await _dataService.SaveDataSql(_settings.TableNames.EmployeeTable, employeeInfo.ToInsertSqlCmdParams()))
            {
                var passwordEntry = GetPasswordEntry(employeeInfo);
                if (await _passwordProvider.AddUserToRegistry(passwordEntry))
                {
                    return new AddUserResponse() { 
                        Name = employeeInfo.Name
                    };
                }
            }

            return new AddUserResponse();
        }

        public async Task<AddUserResponse> UpdateEmployeeAsync(EmployeeInfo employeeInfo)
        {
            if (await _dataService.UpdateDataSql(_settings.TableNames.EmployeeTable, employeeInfo.Id, employeeInfo.ToUpdateSqlCmdParams()))
            {
                return new AddUserResponse()
                {
                    Id = employeeInfo.Id,
                    Name = employeeInfo.Name
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
            userInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var passwordInfo = await _passwordProvider.GetUserPassword(userInfo.UserName);
            passwordInfo.Password = req.NewPassword;

            await UpdateEmployeeAsync(userInfo);
            var response = await _passwordProvider.UpdatePasswordAsync(passwordInfo);
            return new SuccessResponse() { Success = true };
        }

        private static PasswordInfo GetPasswordEntry(EmployeeInfo req)
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

        public async Task<EmployeeInfo> GetEmployeeInfoByUserNameAsync(string userName)
        {
            var response = await _dataService.GetDataByUserName<EmployeeInfo>(userName, _settings.TableNames.EmployeeTable);
            if (response != null)
            {
                response.DateOfBirth = response?.DateOfBirth?.ConvertDate();
                response.CreatedDateTime = response?.CreatedDateTime?.ConvertDate();
                response.UpdatedDateTime = response?.UpdatedDateTime?.ConvertDate();
            }
            return response;
        }
    }
}
