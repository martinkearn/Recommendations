using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRecommendations.Models
{
    public class CartItem
    {
        public Sku Sku { get; set; }

        public int Quantity { get; set; }
    }
}
