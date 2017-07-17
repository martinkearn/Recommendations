using Recommendations.Models;

namespace Recommendations.ViewModels
{
    public class HomecatalogItemViewModel
    {
        public CatalogItem catalogItem { get; set; }
        public RecommendedItems ITIItems { get; set; }
        public RecommendedItems FBTItems { get; set; }
    }
}
