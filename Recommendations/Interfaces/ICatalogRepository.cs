using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.Interfaces
{
    public interface ICatalogRepository
    {
        IEnumerable<CatalogItem> GetSkus();

        CatalogItem GetSkuById(string id);

        IEnumerable<string> GetSkuCategories();
    }
}
