using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteDBProvider.Translator
{
    public static class RecordInfoTranslator
    {
        public static string ToInsertSqlCmdParams(this Contracts.Models.RecordEntryInfo req)
        {
            var sqlCmd = "(ClientId, ClientName, EmployeeId, EmployeeName, Municipality, Location, ScannedDateTime)" +
                   "VALUES ( '" + req.ClientId + "', '" + req.ClientName + "', '" + req.EmployeeId + "', '" + req.EmployeeName + "', '" + req.Municipality + "', '" + req.Location + "', '" + req.ScannedDateTime + "')";

            return sqlCmd;
        }
    }
}
