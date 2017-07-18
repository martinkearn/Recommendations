using Recommendations.Dtos;
using Recommendations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IRecommendationsRepository
    {
        Task<IEnumerable<CatalogItem>> GetRecommendations(string ids, string numberOfResults, string minimalScore);
    }
}
