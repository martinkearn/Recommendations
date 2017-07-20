using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Dtos
{
    public class CategoryDto
    {
        public string category { get; set; }

        public string outfitSection { get; set; }
        public IEnumerable<string> footwearCategories { get; set; }
        public IEnumerable<string> legwareCategories { get; set; }
        public IEnumerable<string> bodywareCategories { get; set; }
        public IEnumerable<string> headwearCategories { get; set; }
        public IEnumerable<string> topRelatedCategories { get; set; }
    }
}
