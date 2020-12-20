using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Translator.ServiceModelToDBModel
{
    public static class AdminInfoTranslator
    {
        private static AdminInfo ToDBModel<T>(this Contracts.Models.AdminInfo req, string nextId) 
        {
            var dbModel = new AdminInfo()
            {
                AdminId = nextId,
                AdminName = req.AdminName
            };

            return dbModel;
        }
    }
}
