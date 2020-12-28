using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeInfo>> GetEmployees();
        Task<List<EmployeeInfo>> GetAllEmployees();
        Task<EmployeeInfo> GetEmployeeInfoAsync(string id);
        Task<AddUserResponse> AddEmployee(EmployeeAddRequest employeeAddRequest);
        Task<RemoveUserResponse> RemoveEmployeeInfoByIdAsync(string id);
        Task<AddUserResponse> UpdateEmployeeAsync(EmployeeInfo employeeInfo);
    }
}
