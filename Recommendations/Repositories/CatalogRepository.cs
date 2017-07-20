﻿using Microsoft.AspNetCore.Hosting;
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
                        TopRelatedCategoryTitles = categoryData.topRelatedCategories.ToList()
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
                        TopRelatedCategoryTitles = new List<string>()
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
            return _catalogItems.Where(o => o.Id == id).FirstOrDefault();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categories;
        }

        public Category GetCategoryById(string id)
        {
            return _categories.Where(o => o.Title == id).FirstOrDefault();
        }

        public IEnumerable<string> GetBrands()
        {
            var uniqueBrands = _catalogItems.Select(o => o.Brand).Distinct().ToList();
            return uniqueBrands;
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

        public string GetOutfitSection(CatalogItem catalogItem)
        {
            var category = GetCategoryById(catalogItem.Type);
            return category.OutfitSection;
        }
    }
}
