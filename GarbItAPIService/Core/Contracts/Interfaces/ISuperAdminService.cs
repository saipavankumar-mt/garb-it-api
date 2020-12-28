using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface ISuperAdminService
    {
        Task<SuperAdminInfo> GetSuperAdminInfoAsync();

        Task<AddUserResponse> AddSuperAdminAsync(SuperAdminAddRequest req);
    }
}
