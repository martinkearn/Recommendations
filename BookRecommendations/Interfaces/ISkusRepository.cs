using BookRecommendations.Models;
using System.Collections.Generic;

namespace BookRecommendations.Interfaces
{
    public interface ISkusRepository
    {
        IEnumerable<Sku> GetSkus();

        Sku GetSkuById(string id);
    }
}
