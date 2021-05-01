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
        Task<EmployeeInfo> GetEmployeeInfoByUserNameAsync(string userName);
        Task<AddUserResponse> AddEmployee(EmployeeInfo employeeInfo);
        Task<RemoveUserResponse> RemoveEmployeeInfoByIdAsync(string id);
        Task<AddUserResponse> UpdateEmployeeAsync(EmployeeInfo employeeInfo);
        Task<List<EmployeeInfo>> SearchEmployeesAsync(List<SearchRequest> searchRequests);
        Task<int> GetEmployeeCountAsync(string reportsToId = "");
        Task<SuccessResponse> UpdateEmployeePasswordAsync(UpdatePasswordRequest req);
    }
}
