using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IAdminProvider
    {
        Task<List<AdminInfo>> GetAdmins();

        Task<AddUserResponse> AddAdmin(AdminInfo adminInfo);

        Task<AdminInfo> GetAdminInfoAsync(string id);
        Task<RemoveUserResponse> RemoveAdminInfoByIdAsync(string id);
    }
}
