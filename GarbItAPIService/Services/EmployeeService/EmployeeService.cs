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

        public async Task<List<EmployeeInfo>> GetEmployees()
        {
            return await _employeeProvider.GetEmployees();
        }
    }
}
