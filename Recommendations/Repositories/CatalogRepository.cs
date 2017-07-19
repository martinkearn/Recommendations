using Microsoft.AspNetCore.Hosting;
using Recommendations.Interfaces;
using Recommendations.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;
using Microsoft.Extensions.Options;

namespace Recommendations.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IEnumerable<CatalogItem> _catalogItems;
        private readonly AppSettings _appSettings;

        public CatalogRepository(IHostingEnvironment environment, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            Random random = new Random();
            var catalogItems = new List<CatalogItem>();
            var rootPath = environment.ContentRootPath;
            var storeFilePath = $"{rootPath}/wwwroot/{_appSettings.CatalogFileName}";
            using (var fileStream = new FileStream(storeFilePath, FileMode.Open))
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
        }

        public IEnumerable<CatalogItem> GetCatalogItems()
        {
            return _catalogItems;
        }

        public CatalogItem GetCatalogItemById(string id)
        {
            return _catalogItems.Where(o => o.Id == id).FirstOrDefault();
        }

        public IEnumerable<string> GetCategories()
        {
            var uniqueTypes = _catalogItems.Select(o => o.Type).Distinct().ToList();
            return uniqueTypes;
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
    }
}
