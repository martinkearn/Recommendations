using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class CatalogItemsPartialViewModel
    {
        public List<CatalogItem> CatalogItems { get; set; }

        public int MaxVisible { get; set; }

        public string ItemGridClass { get; set; }
    }
}
