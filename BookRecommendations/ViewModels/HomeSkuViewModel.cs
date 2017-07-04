using BookRecommendations.Models;

namespace BookRecommendations.ViewModels
{
    public class HomeSkuViewModel
    {
        public Sku Sku { get; set; }
        public RecommendedItems ITIItems { get; set; }
        public RecommendedItems FBTItems { get; set; }
    }
}
