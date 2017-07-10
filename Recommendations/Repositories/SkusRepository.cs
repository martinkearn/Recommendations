using Microsoft.AspNetCore.Hosting;
using Recommendations.Interfaces;
using Recommendations.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

namespace Recommendations.Repositories
{
    public class SkusRepository : ISkusRepository
    {
        private readonly IEnumerable<Sku> _skus;

        public SkusRepository(IHostingEnvironment environment)
        {
            Random random = new Random();
            var skus = new List<Sku>();
            var rootPath = environment.ContentRootPath;
            var storeFilePath = rootPath + "/wwwroot/msstore-catalog.txt";
            using (var fileStream = new FileStream(storeFilePath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var cells = line.Split(',');

                        //create a new random price as it is not in the dataset
                        var price = Convert.ToDecimal(random.NextDouble() * (1.00 - 20.00) + 20.00);

                        var sku = new Sku()
                        {
                            Id = cells[0],
                            Title = cells[1],
                            Type = cells[2],
                            Description = string.Empty,
                            Price = price
                        };
                        skus.Add(sku);
                    }
                }
            }
            _skus = skus.AsEnumerable();
        }

        public IEnumerable<Sku> GetSkus()
        {
            return _skus;
        }

        public Sku GetSkuById(string id)
        {
            return _skus.Where(o => o.Id == id).FirstOrDefault();
        }

        public IEnumerable<string> GetSkuCategories()
        {
            var uniqueTypes = _skus.Select(o => o.Type).Distinct().ToList();
            return uniqueTypes;
        }
    }
}
