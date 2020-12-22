using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService
{
    public class AdminService : IAdminService
    {
        private IAdminProvider _adminProvider;
        public AdminService(IAdminProvider adminProvider)
        {
            _adminProvider = adminProvider;
        }

        public async Task<bool> AddAdmin(AdminInfo adminInfo)
        {
            return await _adminProvider.AddAdmin(adminInfo);
        }

        public async Task<AdminInfo> GetAdminInfoAsync(string id)
        {
            return await _adminProvider.GetAdminInfoAsync(id);
        }

        public async Task<List<AdminInfo>> GetAdminInfos()
        {
            return await _adminProvider.GetAdmins();
        }

        public async Task<bool> RemoveAdminInfoByIdAsync(string id)
        {
            return await _adminProvider.RemoveAdminInfoByIdAsync(id);
        }
    }
}
