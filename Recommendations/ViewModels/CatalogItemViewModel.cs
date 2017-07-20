using Recommendations.Dtos;
using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.ViewModels
{
    public class CatalogItemViewModel
    {
        public CatalogItem CatalogItem { get; set; }
        public IEnumerable<CatalogItem> Recommendations { get; set; }

        public string OutfitSection { get; set; }
    }
}
