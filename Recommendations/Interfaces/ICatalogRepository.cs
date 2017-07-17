using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.Interfaces
{
    public interface ICatalogRepository
    {
        IEnumerable<CatalogItem> GetCatalogItems();

        CatalogItem GetCatalogItemById(string id);

        IEnumerable<string> GetCatalogItemCategories();
    }
}
