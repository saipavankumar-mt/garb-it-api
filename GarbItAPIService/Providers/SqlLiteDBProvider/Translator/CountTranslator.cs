using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteDBProvider.Translator
{
    public static class CountTranslator
    {
        public static string ToInsertSqlCmdParams(this Contracts.Models.CountInfo req)
        {
            var sqlCmd = "(Id, Count)" +
                   "VALUES ( '" + req.Id + "', '" + req.Count + "')";

            return sqlCmd;
        }

        public static string ToUpdateSqlCmdParams(this Contracts.Models.CountInfo req)
        {
            var sqlCmd = " Count = '" + req.Count + "'  ";

            return sqlCmd;
        }
    }
}
