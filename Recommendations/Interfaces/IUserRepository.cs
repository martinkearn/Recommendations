using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IUserRepository
    {
        string GetUser();

        string SetUser(string userId);
    }
}
