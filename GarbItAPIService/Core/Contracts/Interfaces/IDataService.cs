using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IDataService
    {
        Task<List<T>> GetData<T>(string tableName);
        Task<bool> UpdateData<T>(T adminInfo, string tableName);
        Task<bool> SaveData<T>(T adminInfo, string tableName);
        Task<T> GetDataById<T>(string adminId, string tableName);

        Task<string> GetNextId(string tableName);
    }
}
