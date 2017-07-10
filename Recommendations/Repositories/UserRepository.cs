using Microsoft.Extensions.Options;
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

        public UserRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GetUser()
        {
            return string.Empty;
        }

        public string SetUser(string userId)
        {
            return GetUser();
        }
    }
}
