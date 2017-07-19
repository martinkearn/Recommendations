using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommendations.Dtos;
using Recommendations.Interfaces;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IEnumerable<RelatedCategoryDTO> _categories;

        private readonly AppSettings _appSettings;

        private string _categoriesFilePath;

        public CategoryRepository(IHostingEnvironment environment, IOptions<AppSettings> appSettings)
        {
            _categories = new List<RelatedCategoryDTO>();
            _appSettings = appSettings.Value;
            var rootPath = environment.ContentRootPath;
            _categoriesFilePath = $"{rootPath}/wwwroot/{_appSettings.CategoriesFileName}";

            string jsonString = File.ReadAllText(_categoriesFilePath);
            var data = JsonConvert.DeserializeObject<List<RelatedCategoryDTO>>(jsonString);
            _categories = data;
        }

        public IEnumerable<string> GetRelatedCategories(string category)
        {
            var match = _categories.FirstOrDefault(i => i.Category == category)?.RelatedCategories;
            return match;
        }
    }
}
