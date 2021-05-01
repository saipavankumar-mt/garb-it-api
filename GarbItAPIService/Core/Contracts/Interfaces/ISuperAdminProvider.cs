using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface ISuperAdminProvider
    {
        Task<SuperAdminInfo> GetSuperAdminInfoAsync(string id);

        Task<SuperAdminInfo> GetSuperAdminInfoByUserNameAsync(string userName);

        Task<AddUserResponse> AddSuperAdmin(SuperAdminInfo superAdminInfo);

        Task<AddUserResponse> UpdateSuperAdminAsync(SuperAdminInfo superAdminInfo);

        Task<SuccessResponse> UpdateSuperAdminPasswordAsync(UpdatePasswordRequest req);

    }
}
