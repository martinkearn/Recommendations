using Recommendations.Interfaces;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        public IEnumerable<CatalogItem> SearchCatalogItems()
        {
            var results = new List<CatalogItem>();
            return results;
        }
    }
}
