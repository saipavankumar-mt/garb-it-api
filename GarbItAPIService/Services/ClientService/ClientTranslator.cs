using Contracts;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientService
{
    public static class ClientTranslator
    {
        public static ClientInfo ToCoreModel(this ClientAddRequest req)
        {
            var userInfo = AmbientContext.Current.UserInfo;
            var clientInfo = new ClientInfo()
            {
                QRCodeId = Guid.NewGuid().ToString(),
                Name = req.Name,
                PhoneNumber = req.PhoneNumber,
                Gender = req.Gender,
                DateOfBirth = req.DateOfBirth,
                Married = req.Married,
                Location = req.Location,
                Municipality = req.Municipality,
                Address = req.Address,
                City = req.City,
                State = req.State,
                Country = req.Country,
                CreatedById = userInfo.Id,
                CreatedByName = userInfo.Name,
                CreatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
                UpdatedById = userInfo.Id,
                UpdatedByName = userInfo.Name,
                UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
            };

            return clientInfo;
        }
    }
}
