﻿using Amazon.DynamoDBv2;
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

namespace AWSDynamoDBProvider.Services
{
    public class AWSDataService : IDataService
    {
        IAmazonDynamoDB _dynamoDbClient;
        DynamoDBOperationConfig _dynamoDbOperationConfig;
        private AWSDynamoDBSettings _settings;

        public AWSDataService(IAmazonDynamoDB dynamoDb, IOptions<AWSDynamoDBSettings> options)
        {
            this._dynamoDbClient = dynamoDb;
            _dynamoDbOperationConfig = new DynamoDBOperationConfig();
            _settings = options.Value;
        }

        public async Task<bool> SaveData<T>(T req, string tableName)
        {
            try
            {                
                using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
                {
                    _dynamoDbOperationConfig.OverrideTableName = tableName;
                    await dbContext.SaveAsync<T>(req, _dynamoDbOperationConfig);
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<List<T>> GetData<T>(string tableName)
        {
            try
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
            catch(Exception ex)
            {
                return new List<T>();
            }
            
        }

        public async Task<List<T>> GetData<T>(string tableName, string relationshipKey, string relationshipId)
        {
            try
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
            catch (Exception ex)
            {
                return new List<T>();
            }

        }

        public Task<bool> UpdateData<T>(T adminInfo, string tableName)
        {
            throw new NotImplementedException();
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
            try
            {
                using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
                {
                    _dynamoDbOperationConfig.OverrideTableName = tableName;
                    await dbContext.DeleteAsync<T>(id, _dynamoDbOperationConfig);
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        public async Task<string> GetNextId(string tableName)
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
                        NextId = 1
                    }, _dynamoDbOperationConfig);
                    nextId = 1;
                }
                else
                {
                    doc.NextId++;
                    await dbContext.SaveAsync(doc, _dynamoDbOperationConfig);
                    nextId = doc.NextId;
                }
            }
            return nextId.ToString();
        }

        
    }
}
