using BookRecommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRecommendations.ViewModels
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }

        public RecommendedItems ITIItems { get; set; }
    }
}
