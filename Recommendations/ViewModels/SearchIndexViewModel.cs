using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class SearchIndexViewModel
    {
        public string Query { get; set; }
        public IEnumerable<CatalogItem> Results { get; set; }
    }
}
