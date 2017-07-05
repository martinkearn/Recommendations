using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRecommendations.Models
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }

        public int TotalQuantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
