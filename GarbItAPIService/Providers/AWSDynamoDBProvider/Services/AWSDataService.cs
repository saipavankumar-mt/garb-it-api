using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DbModel = AWSDynamoDBProvider.Model;
using Contracts.Models;
using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Options;
using AWSDynamoDBProvider.Model;

namespace AWSDynamoDBProvider.Services
{
    public class AWSDataService : IDataService
    {
        IAmazonDynamoDB _dynamoDbClient;
        DynamoDBOperationConfig _dynamoDbOperationConfig;
        private DBSettings _settings;

        public AWSDataService(IAmazonDynamoDB dynamoDb, IOptions<DBSettings> options)
        {
            this._dynamoDbClient = dynamoDb;
            _dynamoDbOperationConfig = new DynamoDBOperationConfig();
            _settings = options.Value;
        }

        public async Task<bool> SaveData<T>(T req, string tableName)
        {
            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                _dynamoDbOperationConfig.OverrideTableName = tableName;
                await dbContext.SaveAsync<T>(req, _dynamoDbOperationConfig);
            }

            return true;
        }

        public async Task<List<T>> GetData<T>(string tableName)
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;

            var result = new List<T>();

            var request = new ScanRequest
            {
                TableName = tableName,
            };

            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                var docResponse = await this._dynamoDbClient.ScanAsync(request);

                foreach (Dictionary<string, AttributeValue> item in docResponse.Items)
                {
                    var doc = Document.FromAttributeMap(item);
                    var typedDoc = dbContext.FromDocument<T>(doc);
                    result.Add(typedDoc);
                }
            }

            return result;

        }

        public async Task<List<T>> GetData<T>(string tableName, string relationshipKey, string relationshipId)
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;

            var result = new List<T>();

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);


            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition(relationshipKey, ScanOperator.Equal, relationshipId);

            ScanOperationConfig config = new ScanOperationConfig()
            {
                Filter = scanFilter
            };

            Search search = table.Scan(config);

            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                do
                {
                    var documentList = await search.GetNextSetAsync();

                    foreach (var document in documentList)
                    {
                        var typedDoc = dbContext.FromDocument<T>(document);
                        result.Add(typedDoc);
                    }

                } while (!search.IsDone);
            }

            return result;
        }

        public async Task<bool> UpdateData<T>(T req, string tableName)
        {
            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                _dynamoDbOperationConfig.OverrideTableName = tableName;
                await dbContext.SaveAsync<T>(req, _dynamoDbOperationConfig);
            }

            return true;
        }

        public async Task<T> GetDataById<T>(string userId, string tableName)
        {
            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                _dynamoDbOperationConfig.OverrideTableName = tableName;
                var doc = await dbContext.LoadAsync<T>(userId, _dynamoDbOperationConfig);
                return doc;
            }
        }

        public async Task<T> GetDataByUserName<T>(string userName, string tableName)
        {
            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                _dynamoDbOperationConfig.OverrideTableName = tableName;
                var doc = await dbContext.LoadAsync<T>(userName, _dynamoDbOperationConfig);
                return doc;
            }
        }

        public async Task<bool> RemoveDataByIdAsync<T>(string id, string tableName)
        {
            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                _dynamoDbOperationConfig.OverrideTableName = tableName;
                await dbContext.DeleteAsync<T>(id, _dynamoDbOperationConfig);
                return true;
            }
        }

        public async Task<int> GetDataCount(string tableName)
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);
            var count = table.Scan(new ScanOperationConfig()).Count;

            await Task.Delay(0);
            return count;
        }

        public async Task<int> GetDataCount(string tableName, string filterKey, string filterValue)
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;

            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition(filterKey, ScanOperator.Equal, filterValue);

            ScanOperationConfig config = new ScanOperationConfig()
            {
                Filter = scanFilter
            };

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);
            var count = table.Scan(config).Count;

            await Task.Delay(0);
            return count;
        }

        public async Task<int> GetDataCountByDateRange(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null, string idKey = "")
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;

            var scanFilter = BuildScanFilter(searchRequests);
            scanFilter.AddCondition(dateKey, ScanOperator.Between, new DynamoDBEntry[] { fromDate.ToString("yyyy/MM/dd HH:mm"), toDate.ToString("yyyy/MM/dd HH:mm") });

            ScanOperationConfig config = new ScanOperationConfig()
            {
                Filter = scanFilter
            };

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);
            var count = table.Scan(config).Count;

            await Task.Delay(0);
            return count;
        }

        public async Task<(List<T>, string)> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken = "")
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;
            var result = new List<T>();
            string paginationResultToken = "";

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);

            var scanFilter = BuildScanFilter(searchRequests);

            ScanOperationConfig config = new ScanOperationConfig()
            {
                Filter = scanFilter,
                Limit = limit,
                ConsistentRead = true
            };

            if (!string.IsNullOrEmpty(paginationToken))
            {
                config.PaginationToken = paginationToken;
            }

            Search search = table.Scan(config);


            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                var documentList = await search.GetNextSetAsync();
                paginationResultToken = search.PaginationToken;

                foreach (var document in documentList)
                {
                    var typedDoc = dbContext.FromDocument<T>(document);
                    result.Add(typedDoc);
                }
            }

            return (result, paginationResultToken);
        }

        public async Task<List<T>> SearchData<T>(string tableName, List<SearchRequest> searchRequests = null)
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;
            var result = new List<T>();

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);

            var scanFilter = BuildScanFilter(searchRequests);

            ScanOperationConfig config = new ScanOperationConfig()
            {
                Filter = scanFilter,
                ConsistentRead = true
            };

            Search search = table.Scan(config);


            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                do
                {
                    var documentList = await search.GetNextSetAsync();

                    foreach (var document in documentList)
                    {
                        var typedDoc = dbContext.FromDocument<T>(document);
                        result.Add(typedDoc);
                    }

                } while (!search.IsDone);
            }

            return result;
        }

        public async Task<(List<T>, string)> QueryDataByPagination<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null, int limit = 200, string paginationToken="", string idKey = "")
        {
            string paginationResultToken = "";
            _dynamoDbOperationConfig.OverrideTableName = tableName;
            var result = new List<T>();

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);

            if(searchRequests!=null && searchRequests.Count >0)
            {
                var scanFilter = BuildScanFilter(searchRequests);
                scanFilter.AddCondition(dateKey, ScanOperator.Between, new DynamoDBEntry[] { fromDate.ToString("yyyy/MM/dd HH:mm"), toDate.ToString("yyyy/MM/dd HH:mm") });


                ScanOperationConfig config = new ScanOperationConfig()
                {
                    Filter = scanFilter,
                    Limit = limit,
                    ConsistentRead = true
                };


                if (!string.IsNullOrEmpty(paginationToken))
                {
                    config.PaginationToken = paginationToken;
                }

                Search search = table.Scan(config);

                using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
                {
                    var documentList = await search.GetNextSetAsync();
                    paginationResultToken = search.PaginationToken;

                    foreach (var document in documentList)
                    {
                        var typedDoc = dbContext.FromDocument<T>(document);
                        result.Add(typedDoc);
                    }

                }
            }
            else
            {
                QueryFilter queryFilter = new QueryFilter();
                queryFilter.AddCondition(dateKey, QueryOperator.Between, new DynamoDBEntry[] { fromDate.ToString("yyyy/MM/dd HH:mm"), toDate.ToString("yyyy/MM/dd HH:mm") });

                QueryOperationConfig config = new QueryOperationConfig()
                {
                    Filter = queryFilter,
                    Limit = limit,
                    ConsistentRead = true,
                };


                if (!string.IsNullOrEmpty(paginationToken))
                {
                    config.PaginationToken = paginationToken;
                }

                Search search = table.Query(config);

                using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
                {
                    var documentList = await search.GetNextSetAsync();
                    paginationResultToken = search.PaginationToken;

                    foreach (var document in documentList)
                    {
                        var typedDoc = dbContext.FromDocument<T>(document);
                        result.Add(typedDoc);
                    }

                }
            }

            return (result, paginationResultToken);
        }

        public async Task<List<T>> ExportData<T>(string tableName, string dateKey, DateTime fromDate, DateTime toDate, List<SearchRequest> searchRequests = null)
        {
            _dynamoDbOperationConfig.OverrideTableName = tableName;
            var result = new List<T>();

            Table table = Table.LoadTable(this._dynamoDbClient, tableName);

            var scanFilter = BuildScanFilter(searchRequests);
            scanFilter.AddCondition(dateKey, ScanOperator.Between, new DynamoDBEntry[] { fromDate.ToString("yyyy/MM/dd HH:mm"), toDate.ToString("yyyy/MM/dd HH:mm") });

            ScanOperationConfig config = new ScanOperationConfig()
            {
                Filter = scanFilter,
            };

            Search search = table.Scan(config);

            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                do
                {
                    var documentList = await search.GetNextSetAsync();

                    foreach (var document in documentList)
                    {
                        var typedDoc = dbContext.FromDocument<T>(document);
                        result.Add(typedDoc);
                    }

                } while (!search.IsDone);

            }

            return result;
        }

        public async Task<string> GetNextId(string tableName, string prefix, int initialNextId, string decimalFactor)
        {
            int nextId;
            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                _dynamoDbOperationConfig.OverrideTableName = _settings.TableNames.IdGeneratorTable;
                var doc = await dbContext.LoadAsync<DbModel.IdGenerator>(tableName, _dynamoDbOperationConfig);

                if (doc == null)
                {
                    await dbContext.SaveAsync(new DbModel.IdGenerator()
                    {
                        Table = tableName,
                        NextId = initialNextId
                    }, _dynamoDbOperationConfig);
                    nextId = initialNextId;
                }
                else
                {
                    doc.NextId++;
                    await dbContext.SaveAsync(doc, _dynamoDbOperationConfig);
                    nextId = doc.NextId;
                }
            }
            return prefix + nextId.ToString(decimalFactor);
        }

        private static ScanFilter BuildScanFilter(List<SearchRequest> searchRequests)
        {
            ScanFilter scanFilter = new ScanFilter();
            if (searchRequests != null && searchRequests.Count > 0)
            {
                foreach (var item in searchRequests)
                {
                    if (!string.IsNullOrEmpty(item.SearchByKey) && !string.IsNullOrEmpty(item.SearchByValue))
                    {
                        var op = ScanOperator.Equal;
                        if (item.SearchByKey == "Name")
                        {
                            op = ScanOperator.Contains;
                        }

                        scanFilter.AddCondition(item.SearchByKey, op, item.SearchByValue);
                    }
                }
            }

            return scanFilter;
        }

        private static QueryFilter BuildQueryFilter(List<SearchRequest> searchRequests)
        {
            QueryFilter queryFilter = new QueryFilter();
            if (searchRequests != null && searchRequests.Count > 0)
            {
                foreach (var item in searchRequests)
                {
                    if (!string.IsNullOrEmpty(item.SearchByKey) && !string.IsNullOrEmpty(item.SearchByValue))
                    {
                        var op = QueryOperator.Equal;
                        if (item.SearchByKey == "Name")
                        {
                            op = QueryOperator.BeginsWith;
                        }

                        queryFilter.AddCondition(item.SearchByKey, op, item.SearchByValue);
                    }
                }
            }

            return queryFilter;
        }

        public Task<bool> UpdateDataSql(string tableName, string id, string cmdParams)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveDataSql<T>(string tableName, string cmdParams)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveDataSql(string tableName, string cmdParams)
        {
            throw new NotImplementedException();
        }

        public void SetDataBaseSource(string databaseSource)
        {
            throw new NotImplementedException();
        }
    }
}
