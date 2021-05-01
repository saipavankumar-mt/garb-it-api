using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteDBProvider.Translator
{
    public static class ClientInfoTranslator
    {
        public static string ToInsertSqlCmdParams(this Contracts.Models.ClientInfo req)
        {
            var sqlCmd = "(QRCodeId, Name, PhoneNumber, Gender, DateOfBirth, Married, Address, Location, Municipality, City, State, Country, CreatedDateTime, UpdatedDateTime, CreatedById, CreatedByName, UpdatedById, UpdatedByName)" +
                   "VALUES ( '" + req.QRCodeId + "', '" + req.Name + "', '" + req.PhoneNumber + "', '" + req.Gender + "', '" + req.DateOfBirth + "', '" + req.Married + "', '" + req.Address.ToString() + "', '" + req.Location + "', '" + req.Municipality + "', '" + req.City + "', '" + req.State + "', '" + req.Country + "', '" + req.CreatedDateTime + "', '" + req.UpdatedDateTime + "', '" + req.CreatedById + "', '" + req.CreatedByName + "', '" + req.UpdatedById + "', '" + req.UpdatedByName + "')";

            return sqlCmd;
        }

        public static string ToUpdateSqlCmdParams(this Contracts.Models.ClientInfo req)
        {
            var sqlCmd = " QRCodeId = '" + req.QRCodeId + "', Name='" + req.Name + "', PhoneNumber = '" + req.PhoneNumber + "', Gender = '" + req.Gender + "', DateOfBirth = '" + req.DateOfBirth + "', Married = '" + req.Married + "', Address = '" + req.Address + "', Location = '" + req.Location + "', Municipality = '" + req.Municipality + "', City ='" + req.City + "', State = '" + req.State + "', Country = '" + req.Country + "', CreatedDateTime = '" + req.CreatedDateTime + "', UpdatedDateTime = '" + req.UpdatedDateTime + "', CreatedById = '" + req.CreatedById + "', CreatedByName = '" + req.CreatedByName + "', UpdatedById = '" + req.UpdatedById + "', UpdatedByName = '" + req.UpdatedByName + "'  ";

            return sqlCmd;
        }
    }
}
