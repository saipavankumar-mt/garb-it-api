using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteDBProvider.Translator
{
    public static class SuperAdminInfoTranslator
    {
        public static string ToInsertSqlCmdParams(this Contracts.Models.SuperAdminInfo req)
        {
            var sqlCmd = " (Username, Password, Name, PhoneNumber, Gender, DateOfBirth, Married, Role, Designation, Department, Location, Municipality, City, State, Country, CreatedDateTime, UpdatedDateTime, CreatedById, CreatedByName, UpdatedById, UpdatedByName) " +
                   " VALUES ( '" + req.UserName + "', '" + req.Password + "', '" + req.Name + "', '" + req.PhoneNumber + "', '" + req.Gender + "', '" + req.DateOfBirth + "', '" + req.Married + "', '" + req.Role.ToString() + "', '" + req.Designation + "', '" + req.Department + "', '" + req.Location + "', '" + req.Municipality + "', '" + req.City + "', '" + req.State + "', '" + req.Country + "', '" + req.CreatedDateTime + "', '" + req.UpdatedDateTime + "', '" + req.CreatedById + "', '" + req.CreatedByName + "', '" + req.UpdatedById + "', '" + req.UpdatedByName + "')";

            return sqlCmd;
        }

        public static string ToUpdateSqlCmdParams(this Contracts.Models.SuperAdminInfo req)
        {
            var sqlCmd = " Username = '" + req.UserName + "', Password = '" + req.Password + "', Name='" + req.Name + "', PhoneNumber = '" + req.PhoneNumber + "', Gender = '" + req.Gender + "', DateOfBirth = '" + req.DateOfBirth + "', Married = '" + req.Married + "', Role = '" + req.Role.ToString() + "', Designation = '" + req.Designation + "', Department = '" + req.Department + "', Location = '" + req.Location + "', Municipality = '" + req.Municipality + "', City ='" + req.City + "', State = '" + req.State + "', Country = '" + req.Country + "', CreatedDateTime = '" + req.CreatedDateTime + "', UpdatedDateTime = '" + req.UpdatedDateTime + "', CreatedById = '" + req.CreatedById + "', CreatedByName = '" + req.CreatedByName + "', UpdatedById = '" + req.UpdatedById + "', UpdatedByName = '" + req.UpdatedByName + "'  ";

            return sqlCmd;
        }
    }
}
