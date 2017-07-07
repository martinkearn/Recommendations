using BookRecommendations.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookRecommendations.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Sku> Skus { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int NextPage { get; set; }

        public int PreviousPage { get; set; }

        public int TotalSkus { get; set; }
    }
}
