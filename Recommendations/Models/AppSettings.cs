namespace Recommendations.Models
{
    public class AppSettings
    {
        public string RecommendationsApiBaseUrl { get; set; }
        public string RecommendationsApiKey { get; set; }
        public string RecommendationsApiModelId { get; set; }
        public string RecommendationsApiBuildId { get; set; }

        public string CatalogFileName { get; set; }

        public string CategoriesFileName { get; set; }
        public string FreeShippingThreshold { get; set; }
        public string ItemsPerPage { get; set; }
    }
}
