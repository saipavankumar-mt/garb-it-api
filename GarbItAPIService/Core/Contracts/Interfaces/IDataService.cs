﻿using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IDataService
    {
        Task<List<T>> GetData<T>(string tableName);
        Task<List<T>> GetData<T>(string tableName, string relationshipKey, string relationshipId);
        Task<bool> UpdateData<T>(T req, string tableName);
        Task<bool> SaveData<T>(T req, string tableName);
        Task<T> GetDataById<T>(string id, string tableName);
        Task<T> GetDataByUserName<T>(string userName, string tableName);
        Task<string> GetNextId(string tableName, int initialNextId);
        Task<bool> RemoveDataByIdAsync<T>(string id, string tableName);
    }
}
