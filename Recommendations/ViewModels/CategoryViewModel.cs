using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class CategoryViewModel : CatalogItemGroupViewModel
    {
        public IEnumerable<string> RelatedCategoryTitles { get; set; }
        public string OutfitSection { get; set; }
        public IEnumerable<string> RelatedAccessoryTitles { get; set; }
    }
}
