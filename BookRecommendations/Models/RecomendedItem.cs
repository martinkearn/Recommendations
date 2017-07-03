using System.Collections.Generic;

namespace BookRecommendations.Models
{
    public class RecommendedItem
    {
        public List<Item> items { get; set; }
        public double rating { get; set; }
        public List<string> reasoning { get; set; }
    }
}
