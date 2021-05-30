using Contracts.Interfaces;
using Contracts.Models;
using Dapper;
using Microsoft.Extensions.Options;
using SqLiteDBProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDBProvider.Services
{
    public class SQLiteDataService : IDataService
    {
        private DBSettings _settings;
        private string databaseLocation;

        public SQLiteDataService(IOptions<DBSettings> options)
        {
            _settings = options.Value;
            databaseLocation = _settings.DatabaseLocation.GarbitDatabase;
        }

        public void SetDataBaseSource(string databaseSource)
        {
            databaseLocation = databaseSource;
        }

        public async Task<List<T>> ExportData<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null)
        {
            var sqlCommand = string.Format("SELECT * from {0} WHERE ({1} BETWEEN '{2}' AND '{3}')", tableName, dateKey, fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"));

            if (searchRequests != null && searchRequests.Count > 0)
            {
                foreach (var item in searchRequests)
                {
                    sqlCommand += (string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                }
            }

            sqlCommand += string.Format("ORDER BY RecordId DESC");

            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }
            }

            await Task.Delay(0);
            return lst;
        }

        public async Task<List<T>> GetData<T>(string tableName)
        {
            var sqlCommand = string.Format("Select * from {0}", tableName);
            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }
            }

            await Task.Delay(0);
            return lst;
        }

        public async Task<List<T>> GetData<T>(string tableName, string relationshipKey, string relationshipId)
        {
            var sqlCommand = string.Format("Select * from {0} where {1} = '{2}'", tableName, relationshipKey, relationshipId);

            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }

            }

            await Task.Delay(0);
            return lst;
        }

        public async Task<T> GetDataById<T>(string id, string tableName)
        {
            var sqlCommand = string.Format("Select * from {0} where Id = '{1}'", tableName, id);
            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }
            }

            await Task.Delay(0);
            return lst.FirstOrDefault();
        }

        public async Task<T> GetDataByUserName<T>(string userName, string tableName)
        {
            var sqlCommand = string.Format("Select * from {0} where UserName = '{1}'", tableName, userName);
            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }
            }

            await Task.Delay(0);
            return lst.FirstOrDefault();
        }

        public async Task<int> GetDataCount(string tableName)
        {
            var sqlCommand = string.Format("SELECT COUNT(Id) from {0}", tableName);
            using (SQLiteClient _client = new SQLiteClient(databaseLocation))
            {
                var result = _client.RunCountSQL(sqlCommand);
                await Task.Delay(0);
                return result;
            }

        }

        public async Task<int> GetDataCount(string tableName, string filterKey, string filterValue)
        {
            var sqlCommand = string.Format("SELECT COUNT(Id) from {0} where {1} = '{2}'", tableName, filterKey, filterValue);
            using (SQLiteClient _client = new SQLiteClient(databaseLocation))
            {
                var result = _client.RunCountSQL(sqlCommand);

                await Task.Delay(0);
                return result;
            }
        }

        public async Task<int> GetDataCountByDateRange(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null, string idKey = "")
        {
            idKey = string.IsNullOrEmpty(idKey) ? "Id" : idKey;

            var sqlCommand = string.Format("SELECT COUNT({0}) from {1} where ({2} BETWEEN '{3}' AND '{4}')", idKey, tableName, dateKey, fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"));

            if (searchRequests != null && searchRequests.Count > 0)
            {
                foreach (var item in searchRequests)
                {
                    sqlCommand += (string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                }
            }
            using (SQLiteClient _client = new SQLiteClient(databaseLocation))
            {
                var result = _client.RunCountSQL(sqlCommand);
                await Task.Delay(0);
                return result;
            }

        }

        public async Task<bool> RemoveDataByIdAsync<T>(string id, string tableName)
        {
            var sqlCommand = string.Format("DELETE from {0} where Id = '{1}'", tableName, id);
            using (SQLiteClient _client = new SQLiteClient(databaseLocation))
            {
                var result = _client.RunInsertOrUpdateOrDeleteSQL(sqlCommand);

                await Task.Delay(0);
                return result; 
            }
        }


        public async Task<bool> SaveDataSql(string tableName, string cmdParams)
        {
            var sqlCommand = string.Format("INSERT INTO {0}{1}", tableName, cmdParams);
            using (SQLiteClient _client = new SQLiteClient(databaseLocation))
            {
                var result = _client.RunInsertOrUpdateOrDeleteSQL(sqlCommand);

                await Task.Delay(0);
                return result; 
            }
        }


        public async Task<bool> UpdateDataSql(string tableName, string id, string cmdParams)
        {
            var sqlCommand = string.Format("UPDATE {0} SET {1} WHERE Id={2}", tableName, cmdParams, id);
            using (SQLiteClient _client = new SQLiteClient(databaseLocation))
            {
                var result = _client.RunInsertOrUpdateOrDeleteSQL(sqlCommand);

                await Task.Delay(0);
                return result; 
            }
        }


        public async Task<(List<T>, string)> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken = "")
        {

            var offset = string.IsNullOrEmpty(paginationToken) ? 0 : Convert.ToInt32(paginationToken);

            var sqlCommand = string.Format("SELECT * from {0}", tableName);

            if (searchRequests != null && searchRequests.Count > 0)
            {
                int i = 0;
                foreach (var item in searchRequests)
                {
                    if (i == 0)
                    {
                        sqlCommand += (string.Format(" WHERE ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                        i++;
                    }
                    else
                    {
                        sqlCommand += (string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                    }
                }
            }

            sqlCommand += string.Format("ORDER BY Id DESC LIMIT {0} OFFSET {1}", limit, offset);

            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }
            }

            await Task.Delay(0);
            return (lst, lst.Count.ToString());
        }

        public async Task<List<T>> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null)
        {
            var sqlCommand = string.Format("SELECT * from {0}", tableName);

            if (searchRequests != null && searchRequests.Count > 0)
            {
                int i = 0;
                foreach (var item in searchRequests)
                {
                    if (i == 0)
                    {
                        sqlCommand += (string.Format(" WHERE ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                        i++;
                    }
                    else
                    {
                        sqlCommand += (string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                    }
                }
            }

            sqlCommand += string.Format("ORDER BY Id DESC");

            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }
            }

            await Task.Delay(0);
            return lst;
        }


        public async Task<(List<T>, string)> QueryDataByPagination<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken = "", string idKey = "")
        {
            var offset = string.IsNullOrEmpty(paginationToken) ? 0 : Convert.ToInt32(paginationToken);
            idKey = string.IsNullOrEmpty(idKey) ? "Id" : idKey;

            var sqlCommand = string.Format("SELECT * from {0} WHERE ({1} BETWEEN '{2}' AND '{3}')", tableName, dateKey, fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"));

            if (searchRequests != null && searchRequests.Count > 0)
            {
                foreach (var item in searchRequests)
                {
                    sqlCommand += (string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                }
            }

            sqlCommand += string.Format("ORDER BY {0} DESC LIMIT {1} OFFSET {2}", idKey, limit, offset);

            var lst = new List<T>();

            using (var client = new SQLiteClient(databaseLocation))
            {
                var result = client.RunSelectSQL(sqlCommand);

                if (result != null)
                {
                    var parser = result.GetRowParser<T>(typeof(T));

                    while (result.Read())
                    {
                        lst.Add(parser(result));
                    }
                }
            }

            await Task.Delay(0);
            return (lst, lst.Count.ToString());
        }

        public Task<string> GetNextId(string tableName, string prefix, int initialNextId, string decimalFactor = "D4")
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveData<T>(T req, string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateData<T>(T req, string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
