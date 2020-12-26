using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class ClientInfoTranslator
    {
        public static ClientInfo ToDBModel(this Contracts.Models.ClientInfo req, string nextId, string qrCodeId)
        {
            var dbModel = new ClientInfo()
            {
                ClientId = nextId,
                Name = req.Name,
                PhoneNumber = req.PhoneNumber,
                Address = req.Address,
                Location = req.Location,
                Muncipality = req.Muncipality,
                City = req.City,
                State = req.State,
                Country = req.Country,
                QRCodeId = qrCodeId,
                CreatedById = req.CreatedById,
                CreatedByName = req.CreatedByName,
                UpdatedById = req.UpdatedById,
                UpdatedByName = req.UpdatedByName,
                CreatedDate = req.CreatedDate,
                UpdatedDate = req.UpdatedDate
            };

            return dbModel;
        }
    }
}
