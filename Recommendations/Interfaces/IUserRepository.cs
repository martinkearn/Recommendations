using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IUserRepository
    {
        string GetUser(ISession session);

        string SetUser(ISession session, string userId);
    }
}
