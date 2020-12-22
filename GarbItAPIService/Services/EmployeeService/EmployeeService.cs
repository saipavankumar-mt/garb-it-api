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

        public async Task<bool> AddEmployee(EmployeeInfo employeeInfo)
        {
            return await _employeeProvider.AddEmployee(employeeInfo);
        }

        public async Task<EmployeeInfo> GetEmployeeInfoAsync(string id)
        {
            return await _employeeProvider.GetEmployeeInfoAsync(id);
        }

        public async Task<List<EmployeeInfo>> GetEmployees(string reportsToId)
        {
            return await _employeeProvider.GetEmployees(reportsToId);
        }

        public async Task<bool> RemoveEmployeeInfoByIdAsync(string id)
        {
            return await _employeeProvider.RemoveEmployeeInfoByIdAsync(id);
        }
    }
}
