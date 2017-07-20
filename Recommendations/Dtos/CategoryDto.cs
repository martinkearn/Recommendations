using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Dtos
{
    public class CategoryDto
    {
        public string category { get; set; }
        public IEnumerable<string> FootwearCategories { get; set; }
        public IEnumerable<string> LegwareCategories { get; set; }
        public IEnumerable<string> BodywareCategories { get; set; }
        public IEnumerable<string> HeadwearCategories { get; set; }
        public IEnumerable<string> RelatedCategories { get; set; }
    }
}
