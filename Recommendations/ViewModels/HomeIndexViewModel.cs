using Recommendations.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Recommendations.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<CatalogItem> catalogItems { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int NextPage { get; set; }

        public int PreviousPage { get; set; }

        public int TotalcatalogItems { get; set; }
    }
}
