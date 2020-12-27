using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IEmployeeProvider
    {
        Task<List<EmployeeInfo>> GetEmployees(string reportsToId = "");
        Task<EmployeeInfo> GetEmployeeInfoAsync(string id);
        Task<AddUserResponse> AddEmployee(EmployeeInfo employeeInfo);
        Task<RemoveUserResponse> RemoveEmployeeInfoByIdAsync(string id);

    }
}
