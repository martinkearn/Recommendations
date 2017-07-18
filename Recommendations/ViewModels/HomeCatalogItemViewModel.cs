using Recommendations.Dtos;
using Recommendations.Models;

namespace Recommendations.ViewModels
{
    public class HomeCatalogItemViewModel
    {
        public CatalogItem CatalogItem { get; set; }
        public RecommendedItems ITIItems { get; set; }
    }
}
