using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class UserLoginViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<CatalogItem> Recommendations { get; set; }
    }
}
