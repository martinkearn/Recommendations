using Microsoft.AspNetCore.Hosting;
using Recommendations.Interfaces;
using Recommendations.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommendations.Dtos;

namespace Recommendations.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IEnumerable<CatalogItem> _catalogItems;
        private readonly IEnumerable<Category> _categories;
        private readonly AppSettings _appSettings;

        public CatalogRepository(IHostingEnvironment environment, IOptions<AppSettings> appSettings)
        {
            //get app settings
            _appSettings = appSettings.Value;

            //setup file paths
            var rootPath = environment.ContentRootPath;
            var catalogFilePath = $"{rootPath}/wwwroot/{_appSettings.CatalogFileName}";
            var categoriesFilePath = $"{rootPath}/wwwroot/{_appSettings.CategoriesFileName}";

            //get catalog items
            var catalogItems = new List<CatalogItem>();
            using (var fileStream = new FileStream(catalogFilePath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var cells = line.Split(',');

                        //Add mandatory data from cells to catalog item object
                        var catalogItem = new CatalogItem()
                        {
                            Id = cells[0],
                            Title = cells[1],
                            Type = cells[2],
                        };

                        //Add optional Catalog Item data
                        catalogItem = AddOptionalProperties(catalogItem,cells);

                        catalogItems.Add(catalogItem);
                    }
                }
            }
            _catalogItems = catalogItems.AsEnumerable();

            //get category items
            var categoryTitles = _catalogItems.Select(o => o.Type).Distinct().ToList();
            var categoriesData = JsonConvert.DeserializeObject<List<CategoryDto>>(File.ReadAllText(categoriesFilePath));
            var categories = new List<Category>();
            foreach (var categoryTitle in categoryTitles)
            {
                //check if we have more data
                var categoryData = categoriesData.Where(o => o.category.ToLower() == categoryTitle.ToLower()).FirstOrDefault();
                if (categoryData != null)
                {
                    categories.Add(new Category()
                    {
                        Title = categoryTitle,
                        OutfitSection = categoryData.outfitSection,
                        RelatedBodywareCategoryTitles = categoryData.bodywareCategories.ToList(),
                        RelatedFootwearCategoryTitles = categoryData.footwearCategories.ToList(),
                        RelatedHeadwearCategoryTitles = categoryData.headwearCategories.ToList(),
                        RelatedLegwareCategoryTitles = categoryData.legwareCategories.ToList(),
                        TopRelatedCategoryTitles = categoryData.topRelatedCategories.ToList(),
                        AccessoryCategoryTitles = categoryData.accessoryCategories.ToList()
                    });
                }
                else
                {
                    categories.Add(new Category()
                    {
                        Title = categoryTitle,
                        RelatedBodywareCategoryTitles = new List<string>(),
                        RelatedFootwearCategoryTitles = new List<string>(),
                        RelatedHeadwearCategoryTitles = new List<string>(),
                        RelatedLegwareCategoryTitles = new List<string>(),
                        TopRelatedCategoryTitles = new List<string>(),
                        AccessoryCategoryTitles = new List<string>()
                    });
                }
            }
            _categories = categories;
        }

        public IEnumerable<CatalogItem> GetCatalogItems()
        {
            return _catalogItems;
        }

        public CatalogItem GetCatalogItemById(string id)
        {
            return _catalogItems.FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categories.OrderBy(o=>o.Title);
        }

        public Category GetCategoryById(string id)
        {
            return _categories.FirstOrDefault(o => o.Title == id);
        }

        public IEnumerable<string> GetBrands()
        {
            return _catalogItems.Select(o => o.Brand).Distinct().ToList();
        }

        public string GetOutfitSection(CatalogItem catalogItem)
        {
            return GetCategoryById(catalogItem.Type).OutfitSection;
        }

        public IEnumerable<CatalogItem> GetOutfit(CatalogItem seedItem, IEnumerable<CatalogItem> recommendations)
        {
            var category = GetCategoryById(seedItem.Type);

            var outfit = new List<CatalogItem>();

            var headwear = recommendations.FirstOrDefault(ri => category?.RelatedHeadwearCategoryTitles?.Any(c => c == ri.Type) ?? false);
            if (headwear != null) outfit.Add(headwear);

            var bodywear = recommendations.FirstOrDefault(ri => category?.RelatedBodywareCategoryTitles?.Any(c => c == ri.Type) ?? false);
            if (bodywear != null) outfit.Add(bodywear);

            var legwear = recommendations.FirstOrDefault(ri => category?.RelatedLegwareCategoryTitles?.Any(c => c == ri.Type) ?? false);
            if (legwear != null) outfit.Add(legwear);

            var footwear = recommendations.FirstOrDefault(ri => category?.RelatedFootwearCategoryTitles?.Any(c => c == ri.Type) ?? false);
            if (footwear != null) outfit.Add(footwear);

            return outfit;
        }

        public IEnumerable<CatalogItem> GetLikeThisButCheaper(CatalogItem seedItem, IEnumerable<CatalogItem> recommendations, decimal percentageCheaper)
        {
            var maxPrice = seedItem.Sell - ((seedItem.Sell / 100) * percentageCheaper);
            var cheaperRecommendations = recommendations
                .Where(o => o.Type == seedItem.Type)
                .Where(o => o.Sell <= maxPrice);
            return cheaperRecommendations;
        }

        public IEnumerable<CatalogItem> GetTargetPrice(IEnumerable<CatalogItem> recommendations, decimal targetPrice)
        {
            return recommendations.Where(o => o.Sell <= targetPrice).OrderByDescending(o=>o.Sell);
        }

        public IEnumerable<CatalogItem> GetRelatedAccesories(IEnumerable<CatalogItem> seedItems, IEnumerable<CatalogItem> recommendations)
        {
            var relatedAccesories = new List<CatalogItem>();

            foreach (var seedItem in seedItems)
            {
                var categoryForItem = GetCategoryById(seedItem.Type);
                var relatedAccesoriesForItem = recommendations.Where(ri => categoryForItem?.AccessoryCategoryTitles?.Any(c => c == ri.Type) ?? false);
                relatedAccesories.AddRange(relatedAccesoriesForItem);
            }

            return relatedAccesories;
        }

        private CatalogItem AddOptionalProperties(CatalogItem catalogItem, string[] cells)
        {
            catalogItem.Description = (cells.Count() >= 4) ?
                cells[3] :
                string.Empty;

            catalogItem.Rrp = (cells.Count() >= 5) ?
                Convert.ToDecimal(cells[4].Substring(4)) :
                Convert.ToDecimal(0.00);

            catalogItem.Sell = (cells.Count() >= 6) ?
                Convert.ToDecimal(cells[5].Substring(5)) :
                Convert.ToDecimal(0.00);

            catalogItem.Brand = (cells.Count() >= 7) ?
                cells[6].Substring(6) :
                string.Empty;

            catalogItem.ImageUrl = (cells.Count() >= 8) ?
                cells[7].Substring(4) :
                "http://lorempixel.com/500/500/";

            catalogItem.Colour = (cells.Count() >= 9) ?
                cells[8].Substring(5) :
                string.Empty;

            return catalogItem;
        }
    }
}
