using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class ClientInfoTranslator
    {
        public static ClientInfo ToDBModel(this Contracts.Models.ClientInfo req, string nextId = "")
        {
            var dbModel = new ClientInfo()
            {
                Id = string.IsNullOrEmpty(req.Id) ? nextId : req.Id,
                Name = req.Name,
                PhoneNumber = req.PhoneNumber,
                Gender = req.Gender,
                DateOfBirth = req.DateOfBirth,
                Married = req.Married,
                Address = req.Address,
                Location = req.Location,
                Municipality = req.Municipality,
                City = req.City,
                State = req.State,
                Country = req.Country,
                QRCodeId = string.IsNullOrEmpty(req.QRCodeId) ?  Guid.NewGuid().ToString() : req.QRCodeId,
                CreatedById = req.CreatedById,
                CreatedByName = req.CreatedByName,
                UpdatedById = req.UpdatedById,
                UpdatedByName = req.UpdatedByName,
                CreatedDateTime = req.CreatedDateTime,
                UpdatedDateTime = req.UpdatedDateTime
            };

            return dbModel;
        }
    }
}
