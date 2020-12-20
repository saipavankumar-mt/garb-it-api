using Amazon.DynamoDBv2.Model;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Translator.DBModelToServiceModel
{
    public static class AdminInfoTranslator
    {
        public static List<T> ToEntityModel<T>(this ScanResponse queryRs)
        {
            var response = new List<T>();            

            foreach (var item in queryRs.Items)
            {
                var obj = item.ToEntityModel();

                response.Add((T)Convert.ChangeType(obj, typeof(T)));
            }
            return response;
        }

        private static AdminInfo ToEntityModel(this Dictionary<string, AttributeValue> dic)
        {
            var res = new AdminInfo()
            {
                AdminId = dic["AdminId"].S,
                AdminName = dic["AdminName"].S
            };
            return res;
        }
    }
}
