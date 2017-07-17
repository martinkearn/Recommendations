using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.Interfaces
{
    public interface ICatalogRepository
    {
        IEnumerable<CatalogItem> GetcatalogItems();

        CatalogItem GetcatalogItemById(string id);

        IEnumerable<string> GetCatalogItemCategories();
    }
}
