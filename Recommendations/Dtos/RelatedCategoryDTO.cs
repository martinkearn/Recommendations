using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Dtos
{
    public class RelatedCategoryDTO
    {
        public string Category { get; set; }
        public IEnumerable<string> FootwearCategory { get; set; }
        public IEnumerable<string> LegwareCategory { get; set; }
        public IEnumerable<string> BodywareCategory { get; set; }
        public IEnumerable<string> HeadwearCategory { get; set; }
        public IEnumerable<string> RelatedCategories { get; set; }
    }
}
