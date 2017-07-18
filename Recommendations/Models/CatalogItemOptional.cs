using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Models
{
    public partial class CatalogItem
    {
        public string Description { get; set; }
        public decimal Rrp { get; set; }
        public decimal Sell { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; }
        public string Colour { get; set; }
        public IEnumerable<CatalogItem> Recommendations { get; set; }
    }
}
