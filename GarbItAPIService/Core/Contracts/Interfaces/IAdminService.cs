using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminInfo>> GetAdminInfos();

        Task<bool> AddAdmin(AdminInfo adminInfo);

        Task<AdminInfo> GetAdminInfoAsync(string id);
        Task<bool> RemoveAdminInfoByIdAsync(string id);
    }
}
