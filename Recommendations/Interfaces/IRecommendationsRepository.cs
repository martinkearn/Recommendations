using Recommendations.Dtos;
using Recommendations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IRecommendationsRepository
    {
        Task<IEnumerable<CatalogItem>> GetRecommendations(List<CatalogItem> seedItems, string numberOfResults, string minimalScore);

        Task<IEnumerable<CatalogItem>> GetPersonalizedRecommendedItems(string userId, string numberOfResults);
    }
}
