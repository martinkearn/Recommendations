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

                        //Get/create optional Catalog Item data
                        var description = string.Empty;
                        var price = Convert.ToDecimal(random.NextDouble() * (1.00 - 20.00) + 20.00);

                        var catalogItem = new CatalogItem()
                        {
                            Id = cells[0],
                            Title = cells[1],
                            Type = cells[2],
                            Description = description,
                            Sell = price
                        };
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

        public IEnumerable<string> GetCatalogItemCategories()
        {
            var uniqueTypes = _catalogItems.Select(o => o.Type).Distinct().ToList();
            return uniqueTypes;
        }
    }
}
