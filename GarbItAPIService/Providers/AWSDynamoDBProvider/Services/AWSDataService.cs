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
using AWSDynamoDBProvider.Translator.DBModelToServiceModel;

namespace AWSDynamoDBProvider.Services
{
    public class AWSDataService : IDataService
    {
        IAmazonDynamoDB _dynamoDbClient;
        DynamoDBOperationConfig _dynamoDbOperationConfig;

        public AWSDataService(IAmazonDynamoDB dynamoDb)
        {
            this._dynamoDbClient = dynamoDb;
            _dynamoDbOperationConfig = new DynamoDBOperationConfig();            
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

                var request = new ScanRequest
                {
                    TableName = tableName,
                };

                var docResponse = await this._dynamoDbClient.ScanAsync(request);

                return docResponse.ToEntityModel<T>();
            }
            catch(Exception ex)
            {
                return new List<T>();
            }
            
        }

        public Task<bool> UpdateData<T>(T adminInfo, string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetDataById<T>(string adminId, string tableName)
        {
            throw new NotImplementedException();
        }


        public async Task<string> GetNextId(string tableName)
        {
            int nextId;
            using (var dbContext = new DynamoDBContext(this._dynamoDbClient))
            {
                _dynamoDbOperationConfig.OverrideTableName = "IdGenerator";
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
