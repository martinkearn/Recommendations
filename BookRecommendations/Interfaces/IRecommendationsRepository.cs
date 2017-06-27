using BookRecommendations.Models;
using System.Threading.Tasks;

namespace BookRecommendations.Interfaces
{
    public interface IRecommendationsRepository
    {
        Task<RecommendedItems> GetITIItems(string id, string numberOfResults, string minimalScore);
        Task<RecommendedItems> GetFBTItems(string id, string numberOfResults, string minimalScore);
    }
}
