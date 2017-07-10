using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.Interfaces
{
    public interface ISkusRepository
    {
        IEnumerable<Sku> GetSkus();

        Sku GetSkuById(string id);

        IEnumerable<string> GetSkuCategories();
    }
}
