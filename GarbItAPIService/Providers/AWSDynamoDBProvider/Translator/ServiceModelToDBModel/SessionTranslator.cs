using Amazon.Util;
using AWSDynamoDBProvider.Model;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class SessionTranslator
    {
        public static Model.SessionInfo ToDBModel(this Contracts.Models.SessionInfo req, string guid)
        {
            DateTime expirationTime = DateTime.Now.AddMinutes(30);
            var epochSeconds = AWSSDKUtils.ConvertToUnixEpochSeconds(expirationTime);

            var dbModel = new Model.SessionInfo()
            {
                SessionId = guid,
                UserName = req.UserName,
                UserId = req.UserId,
                Role = req.Role.ToString(),
                ExpiryTime = epochSeconds.ToString()
            };

            return dbModel;
        }

        public static Contracts.Models.SessionInfo ToEntityModel(this Model.SessionInfo req)
        {
            var entityModel = new Contracts.Models.SessionInfo()
            {
                SessionId = req.SessionId,
                UserName = req.UserName,
                UserId = req.UserId,
                Role = Enum.Parse<Role>(req.Role)
            };

            return entityModel;
        }
    }
}
