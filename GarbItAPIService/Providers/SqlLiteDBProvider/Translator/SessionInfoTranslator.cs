using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteDBProvider.Translator
{
    public static class SessionInfoTranslator
    {
        public static string ToInsertSqlCmdParams(this Contracts.Models.SessionInfo req)
        {
            var sqlCmd = "(Id, UserName, UserId, UserFullName, Role, SessionCreatedOn)" +
                   "VALUES ( '" + req.Id + "', '" + req.UserName + "', '" + req.UserId + "', '" + req.UserFullName + "', '" + req.Role.ToString() + "', '" + req.SessionCreatedOn + "')";

            return sqlCmd;
        }
    }
}
