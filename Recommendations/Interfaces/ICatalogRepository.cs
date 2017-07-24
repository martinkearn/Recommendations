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

        string GetOutfitSection(CatalogItem catalogItem);

        IEnumerable<CatalogItem> GetOutfit(CatalogItem catalogItem, IEnumerable<CatalogItem> recommendations);

        IEnumerable<CatalogItem> LikeThisButCheaper(CatalogItem seedItem, IEnumerable<CatalogItem> recommendations, decimal percentageCheaper);

        IEnumerable<CatalogItem> TargetPrice(IEnumerable<CatalogItem> recommendations, decimal targetPrice);
    }
}
