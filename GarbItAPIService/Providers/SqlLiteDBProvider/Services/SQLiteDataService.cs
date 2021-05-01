using Contracts.Interfaces;
using Contracts.Models;
using Dapper;
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
        private SQLiteClient _client;

        public SQLiteDataService()
        {
            _client = new SQLiteClient();
        }

        public Task<List<T>> ExportData<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetData<T>(string tableName)
        {
            var sqlCommand = string.Format("Select * from {0}", tableName);
            var lst = new List<T>();

            using (var client = new SQLiteClient())
            {
                var result = client.RunSelectSQL(sqlCommand);
                var parser = result.GetRowParser<T>(typeof(T));

                while (result.Read())
                {
                    lst.Add(parser(result));
                }
            }

            await Task.Delay(0);
            return lst;
        }

        public async Task<List<T>> GetData<T>(string tableName, string relationshipKey, string relationshipId)
        {
            var sqlCommand = string.Format("Select * from {0} where {1} = '{2}'", tableName, relationshipKey, relationshipId);

            var lst = new List<T>();

            using (var client = new SQLiteClient())
            {
                var result = client.RunSelectSQL(sqlCommand);
                var parser = result.GetRowParser<T>(typeof(T));

                while (result.Read())
                {
                    lst.Add(parser(result));
                }
            }

            await Task.Delay(0);
            return lst;
        }

        public async Task<T> GetDataById<T>(string id, string tableName)
        {
            var sqlCommand = string.Format("Select * from {0} where Id = '{1}'", tableName, id);
            var lst = new List<T>();

            using (var client = new SQLiteClient())
            {
                var result = client.RunSelectSQL(sqlCommand);
                var parser = result.GetRowParser<T>(typeof(T));

                while (result.Read())
                {
                    lst.Add(parser(result));
                }
            }

            await Task.Delay(0);
            return lst.FirstOrDefault();
        }

        public async Task<T> GetDataByUserName<T>(string userName, string tableName)
        {
            var sqlCommand = string.Format("Select * from {0} where UserName = '{1}'", tableName, userName);
            var lst = new List<T>();

            using (var client = new SQLiteClient())
            {
                var result = client.RunSelectSQL(sqlCommand);
                var parser = result.GetRowParser<T>(typeof(T));

                while (result.Read())
                {
                    lst.Add(parser(result));
                }
            }

            await Task.Delay(0);
            return lst.FirstOrDefault();
        }

        public async Task<int> GetDataCount(string tableName)
        {
            var sqlCommand = string.Format("SELECT COUNT(Id) from {0}", tableName);
            var result = _client.RunCountSQL(sqlCommand);

            await Task.Delay(0);
            return result;
        }

        public async Task<int> GetDataCount(string tableName, string filterKey, string filterValue)
        {
            var sqlCommand = string.Format("SELECT COUNT(Id) from {0} where {1} = '{2}'", tableName, filterKey, filterValue);
            var result = _client.RunCountSQL(sqlCommand);

            await Task.Delay(0);
            return result;
        }

        public async Task<int> GetDataCountByDateRange(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null)
        {
            var sqlCommand = string.Format("SELECT COUNT(Id) from {0} where ({1} BETWEEN '{2}' AND '{3}')", tableName, dateKey, fromDate, toDate);

            if(searchRequests!=null && searchRequests.Count>0)
            {
                foreach (var item in searchRequests)
                {
                    sqlCommand.Concat(string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                }
            }

            var result = _client.RunCountSQL(sqlCommand);

            await Task.Delay(0);
            return result;
        }

        public async Task<bool> RemoveDataByIdAsync<T>(string id, string tableName)
        {
            var sqlCommand = string.Format("DELETE from {0} where Id = '{1}'", tableName, id);
            var result = _client.RunInsertOrUpdateOrDeleteSQL(sqlCommand);

            await Task.Delay(0);
            return result;
        }


        public async Task<bool> SaveDataSql(string tableName, string cmdParams)
        {
            var sqlCommand = string.Format("INSERT INTO {0}{1}", tableName, cmdParams);
            var result = _client.RunInsertOrUpdateOrDeleteSQL(sqlCommand);

            await Task.Delay(0);
            return result;
        }


        public async Task<bool> UpdateDataSql(string tableName, string id, string cmdParams)
        {
            var sqlCommand = string.Format("UPDATE {0} SET {1} WHERE Id={2}", tableName, cmdParams, id);
            var result = _client.RunInsertOrUpdateOrDeleteSQL(sqlCommand);

            await Task.Delay(0);
            return result;
        }


        public async Task<(List<T>, string)> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken = "")
        {
            var sqlCommand = string.Format("SELECT * from {0} ORDER BY Id LIMIT {1} OFFSET {2} ", tableName, limit, paginationToken);

            if (searchRequests != null && searchRequests.Count > 0)
            {
                int i = 0;
                foreach (var item in searchRequests)
                {
                    if(i==0)
                    {
                        sqlCommand.Concat(string.Format(" where ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                    }
                    else
                    {
                        sqlCommand.Concat(string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                    }
                }
            }

            var lst = new List<T>();

            using (var client = new SQLiteClient())
            {
                var result = client.RunSelectSQL(sqlCommand);
                var parser = result.GetRowParser<T>(typeof(T));

                while (result.Read())
                {
                    lst.Add(parser(result));
                }
            }

            await Task.Delay(0);
            return (lst, lst.Count.ToString());
        }

        public async Task<List<T>> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null)
        {
            var sqlCommand = string.Format("SELECT * from {0} ORDER BY Id ", tableName);

            if (searchRequests != null && searchRequests.Count > 0)
            {
                int i = 0;
                foreach (var item in searchRequests)
                {
                    if (i == 0)
                    {
                        sqlCommand.Concat(string.Format(" where ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                    }
                    else
                    {
                        sqlCommand.Concat(string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                    }
                }
            }

            var lst = new List<T>();

            using (var client = new SQLiteClient())
            {
                var result = client.RunSelectSQL(sqlCommand);
                var parser = result.GetRowParser<T>(typeof(T));

                while (result.Read())
                {
                    lst.Add(parser(result));
                }
            }

            await Task.Delay(0);
            return lst;
        }

        
        public async Task<(List<T>, string)> QueryDataByPagination<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken = "")
        {
            var sqlCommand = string.Format("SELECT * from {0} ORDER BY Id LIMIT {1} OFFSET {2} where ({3} BETWEEN '{4}' AND '{5}')", tableName, limit, paginationToken, dateKey, fromDate, toDate);

            if (searchRequests != null && searchRequests.Count > 0)
            {
                foreach (var item in searchRequests)
                {
                    sqlCommand.Concat(string.Format(" AND ( {0} = '{1}' ) ", item.SearchByKey, item.SearchByValue));
                }
            }

            var lst = new List<T>();

            using (var client = new SQLiteClient())
            {
                var result = client.RunSelectSQL(sqlCommand);
                var parser = result.GetRowParser<T>(typeof(T));

                while (result.Read())
                {
                    lst.Add(parser(result));
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
