using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class CategoryViewModel : CatalogItemGroupViewModel
    {
        public IEnumerable<string> RelatedCategories { get; set; }
    }
}
