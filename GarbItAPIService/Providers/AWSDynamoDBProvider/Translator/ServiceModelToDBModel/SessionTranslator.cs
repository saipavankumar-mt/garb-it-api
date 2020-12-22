using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class SessionTranslator
    {
        public static SessionInfo ToDBModel(this Contracts.Models.SessionInfo req, string guid)
        {
            var dbModel = new SessionInfo()
            {
                SessionId = guid,
                UserName = req.UserName,
                Role = req.Role.ToString(),
                ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(3).ToUnixTimeMilliseconds()
            };

            return dbModel;
        }
    }
}
