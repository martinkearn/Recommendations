using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.Dtos
{
    public class RecommendedItem
    {
        public List<Item> items { get; set; }
        public double rating { get; set; }
        public List<string> reasoning { get; set; }
    }
}
