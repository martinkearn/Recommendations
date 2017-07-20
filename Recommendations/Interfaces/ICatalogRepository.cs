using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.Interfaces
{
    public interface ICatalogRepository
    {
        IEnumerable<CatalogItem> GetCatalogItems();

        CatalogItem GetCatalogItemById(string id);

        IEnumerable<Category> GetCategories();

        Category GetCategoryById(string id);

        IEnumerable<string> GetBrands();
    }
}
