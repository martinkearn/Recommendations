using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Models
{
    public class CartItem
    {
        public CatalogItem Sku { get; set; }

        public int Quantity { get; set; }
    }
}
