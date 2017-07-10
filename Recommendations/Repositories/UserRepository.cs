using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommendations.Interfaces;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppSettings _appSettings;
        private string _userSessionLabel;

        public UserRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _userSessionLabel = "user";
        }

        public string GetUser(ISession session)
        {
            var userData = session.GetString(_userSessionLabel);

            if (userData == null)
            {
                var userId = "Anonymous";
                SaveUser(userId, session);

                //return
                return userId;
            }
            else
            {
                //deserialise and return cart
                var deserializedUser = JsonConvert.DeserializeObject<string>(userData);
                return deserializedUser;
            }
        }

        public string SetUser(ISession session, string userId)
        {
            SaveUser(userId, session);
            return GetUser(session);
        }

        private void SaveUser(string userId, ISession session)
        {
            var serializedUser = JsonConvert.SerializeObject(userId);
            session.SetString(_userSessionLabel, serializedUser);
        }
    }
}
