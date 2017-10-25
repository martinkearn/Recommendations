using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class HomeViewModel
    {
        public List<CatalogItem> FeaturedCatalogItems { get; set; }
        public CatalogItemGroupViewModel CatalogItems { get; set; }
    }
}
