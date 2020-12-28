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
            var adminId = AmbientContext.Current.UserId;
            return await _employeeProvider.GetEmployees(adminId);
        }

        public async Task<List<EmployeeInfo>> GetAllEmployees()
        {
            return await _employeeProvider.GetEmployees();
        }

        public async Task<RemoveUserResponse> RemoveEmployeeInfoByIdAsync(string id)
        {
            return await _employeeProvider.RemoveEmployeeInfoByIdAsync(id);
        }

        public async Task<AddUserResponse> UpdateEmployeeAsync(EmployeeInfo employeeInfo)
        {
            employeeInfo.UpdatedById = AmbientContext.Current.UserId;
            employeeInfo.UpdatedByName = AmbientContext.Current.UserName;
            employeeInfo.UpdatedDateTime = DateTime.Now.ToString();

            return await _employeeProvider.UpdateEmployeeAsync(employeeInfo);
        }
    }
}
