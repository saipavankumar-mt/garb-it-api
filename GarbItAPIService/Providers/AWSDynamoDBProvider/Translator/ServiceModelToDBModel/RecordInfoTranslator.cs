using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class RecordInfoTranslator
    {
        public static ScannedRecordInfo ToDBModel(this Contracts.Models.RecordEntryInfo req, string nextId)
        {
            var dbModel = new ScannedRecordInfo()
            {
                RecordId = nextId,
                ClientId = req.ClientId,
                ClientName = req.ClientName,
                EmployeeId = req.EmployeeId,
                EmployeeName = req.EmployeeName,
                Location = req.Location,
                Municipality = req.Municipality,
                ScannedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm")
            };

            return dbModel;
        }
    }
}
