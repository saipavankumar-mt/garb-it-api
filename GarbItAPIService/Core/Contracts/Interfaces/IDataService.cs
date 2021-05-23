using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IDataService
    {
        void SetDataBaseSource(string databaseSource);
        Task<List<T>> GetData<T>(string tableName);
        Task<List<T>> GetData<T>(string tableName, string relationshipKey, string relationshipId);
        Task<(List<T>, string)> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken = "");

        Task<List<T>> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null);
        Task<(List<T>, string)> QueryDataByPagination<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken = "", string idKey ="");

        Task<List<T>> ExportData<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null);

        Task<bool> UpdateData<T>(T req, string tableName);
        Task<bool> SaveData<T>(T req, string tableName);
        Task<bool> UpdateDataSql(string tableName, string id, string cmdParams);
        Task<bool> SaveDataSql(string tableName, string cmdParams);

        Task<T> GetDataById<T>(string id, string tableName);
        Task<T> GetDataByUserName<T>(string userName, string tableName);
        Task<string> GetNextId(string tableName, string prefix, int initialNextId, string decimalFactor="D4");
        Task<bool> RemoveDataByIdAsync<T>(string id, string tableName);
        Task<int> GetDataCount(string tableName);
        Task<int> GetDataCount(string tableName, string filterKey, string filterValue);
        Task<int> GetDataCountByDateRange(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null, string idKey = "");
    }
}