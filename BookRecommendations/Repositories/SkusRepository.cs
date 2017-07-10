using Microsoft.AspNetCore.Hosting;
using BookRecommendations.Interfaces;
using BookRecommendations.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

namespace BookRecommendations.Repositories
{
    public class SkusRepository : ISkusRepository
    {
        private readonly IEnumerable<Sku> _skus;

        public SkusRepository(IHostingEnvironment environment)
        {
            Random random = new Random();
            var skus = new List<Sku>();
            var rootPath = environment.ContentRootPath;
            var storeFilePath = rootPath + "/wwwroot/bookscatalog.txt";
            using (var fileStream = new FileStream(storeFilePath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var cells = line.Split(',');

                        var year = cells[5].Substring(cells[5].IndexOf('=') + 1);
                        var yearLabel = (year == "0") ?
                            "Year not know" :
                            year;

                        //create a new random price as it is not in the dataset
                        var price = Convert.ToDecimal(random.NextDouble() * (1.00 - 20.00) + 20.00);

                        var sku = new Sku()
                        {
                            Id = cells[0],
                            Title = cells[1],
                            Type = cells[2],
                            Author = cells[3].Substring(cells[3].IndexOf('=') + 1),
                            Publisher = cells[4].Substring(cells[4].IndexOf('=') + 1),
                            Year = yearLabel,
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
