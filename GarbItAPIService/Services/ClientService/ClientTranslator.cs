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
            var clientInfo = new ClientInfo()
            {
                Name = req.Name,
                PhoneNumber = req.PhoneNumber,
                Location = req.Location,
                Municipality = req.Municipality,
                City = req.City,
                State = req.State,
                Country = req.Country,
                CreatedById = AmbientContext.Current.UserId,
                CreatedByName = AmbientContext.Current.UserName,
                CreatedDateTime = DateTime.Now.ToString(),
                UpdatedById = AmbientContext.Current.UserId,
                UpdatedByName = AmbientContext.Current.UserName,
                UpdatedDateTime = DateTime.Now.ToString(),
            };

            return clientInfo;
        }
    }
}
