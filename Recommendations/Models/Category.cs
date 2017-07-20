using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Models
{
    public class Category
    {
        public string Title { get; set; }
        public string OutfitSection { get; set; }
        public List<string> RelatedFootwearCategoryTitles { get; set; }
        public List<string> RelatedLegwareCategoryTitles { get; set; }
        public List<string> RelatedBodywareCategoryTitles { get; set; }
        public List<string> RelatedHeadwearCategoryTitles { get; set; }
        public List<string> TopRelatedCategoryTitles { get; set; }
    }
}
