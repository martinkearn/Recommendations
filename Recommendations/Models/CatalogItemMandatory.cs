namespace Recommendations.Models
{
    public partial class CatalogItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public double RecommendationRating { get; set; }
    }
}
