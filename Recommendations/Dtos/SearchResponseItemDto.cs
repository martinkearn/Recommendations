using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Dtos
{
    public class SearchResponseItemDto
    {
        public double searchscore { get; set; }
        public string Key { get; set; }
        public string Brand { get; set; }
        public string Colour { get; set; }
        public string Description { get; set; }
        public string ID { get; set; }
        public string ImageUrl { get; set; }
        public int Rrp { get; set; }
        public int Sell { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
}
}
