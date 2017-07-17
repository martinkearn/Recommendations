using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class HomeCategoryViewModel
    {
        public string CategoryName { get; set; }

        public IEnumerable<CatalogItem> Skus { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int NextPage { get; set; }

        public int PreviousPage { get; set; }

        public int TotalSkus { get; set; }
    }
}
