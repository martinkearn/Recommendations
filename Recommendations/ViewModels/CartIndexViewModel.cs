using Recommendations.Dtos;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }

        public RecommendedItems ITIItems { get; set; }
    }
}
