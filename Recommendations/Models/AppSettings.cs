namespace Recommendations.Models
{
    public class AppSettings
    {
        public string RecommendationsApiBaseUrl { get; set; }
        public string RecommendationsApiKey { get; set; }
        public string RecommendationsApiModelId { get; set; }
        public string RecommendationsApiBuildId { get; set; }

        public string CatalogFileName { get; set; }
    }
}
