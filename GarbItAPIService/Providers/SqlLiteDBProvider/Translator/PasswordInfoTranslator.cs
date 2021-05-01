using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteDBProvider.Translator
{
    public static class PasswordInfoTranslator
    {
        public static string ToInsertSqlCmdParams(this Contracts.Models.PasswordInfo req)
        {
            var sqlCmd = "(UserName, Name, Password, Role)" +
                   "VALUES ( '" + req.UserName + "', '" + req.Name + "', '" + req.Password + "', '" + req.Role.ToString() + "')";

            return sqlCmd;
        }

        public static string ToUpdateSqlCmdParams(this Contracts.Models.PasswordInfo req)
        {
            var sqlCmd = " Name = '" + req.Name + "', Password = '" + req.Password + "', Role = '" + req.Role + "'  ";

            return sqlCmd;
        }
    }
}
