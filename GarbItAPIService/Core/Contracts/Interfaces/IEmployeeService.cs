using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeInfo>> GetEmployees(string reportsToId);
        Task<EmployeeInfo> GetEmployeeInfoAsync(string id);
        Task<AddUserResponse> AddEmployee(EmployeeInfo employeeInfo);
        Task<AddUserResponse> RemoveEmployeeInfoByIdAsync(string id);
    }
}
