using Recommendations.Models;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IRecommendationsRepository
    {
        Task<RecommendedItems> GetITIItems(string ids, string numberOfResults, string minimalScore);
    }
}
