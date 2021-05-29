using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeProvider _employeeProvider;
        public EmployeeService(IEmployeeProvider employeeProvider)
        {
            _employeeProvider = employeeProvider;
        }

        public async Task<AddUserResponse> AddEmployee(EmployeeAddRequest employeeAddRequest)
        {
            var employeeInfo = employeeAddRequest.ToCoreModel();
            return await _employeeProvider.AddEmployee(employeeInfo);
        }

        public async Task<EmployeeInfo> GetEmployeeInfoAsync(string id)
        {
            return await _employeeProvider.GetEmployeeInfoAsync(id);
        }

        public async Task<List<EmployeeInfo>> GetEmployees()
        {
            var adminId = AmbientContext.Current.UserInfo.Id;
            return await _employeeProvider.GetEmployees(adminId);
        }

        public async Task<List<EmployeeInfo>> GetAllEmployees()
        {
            return await _employeeProvider.GetEmployees();
        }

        public async Task<List<EmployeeInfo>> SearchEmployeesAsync(List<SearchRequest> searchRequests)
        {
            if (searchRequests == null)
            {
                searchRequests = new List<SearchRequest>();
            }

            searchRequests.Add(new SearchRequest()
            {
                SearchByKey = "Municipality",
                SearchByValue = AmbientContext.Current.UserInfo.Municipality
            });

            return await _employeeProvider.SearchEmployeesAsync(searchRequests);
        }

        public async Task<RemoveUserResponse> RemoveEmployeeInfoByIdAsync(string id)
        {
            return await _employeeProvider.RemoveEmployeeInfoByIdAsync(id);
        }

        public async Task<AddUserResponse> UpdateEmployeeAsync(EmployeeInfo employeeInfo)
        {
            employeeInfo.UpdatedById = AmbientContext.Current.UserInfo.Id;
            employeeInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;
            employeeInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            return await _employeeProvider.UpdateEmployeeAsync(employeeInfo);
        }

        public async Task<CountResponse> GetEmployeesCountAsync()
        {
            var count = await _employeeProvider.GetEmployeeCountAsync(AmbientContext.Current.UserInfo.Id);
            return new CountResponse()
            {
                Count = count
            };
        }

        public async Task<SuccessResponse> UpdateEmployeePasswordAsync(UpdatePasswordRequest req)
        {
            return await _employeeProvider.UpdateEmployeePasswordAsync(req);
        }
    }
}
