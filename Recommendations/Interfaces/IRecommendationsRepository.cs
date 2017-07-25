using Recommendations.Dtos;
using Recommendations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IRecommendationsRepository
    {
        Task<IEnumerable<CatalogItem>> GetITIRecommendations(string seedItemId, string numberOfResults, string minimalScore);

        Task<IEnumerable<CatalogItem>> GetPersonalizedRecommendedItemsByUser(string userId, string numberOfResults);
        Task<IEnumerable<CatalogItem>> GetPersonalizedRecommendedItemsByItems(IEnumerable<CatalogItem> seedItems, string numberOfResults);
    }
}
