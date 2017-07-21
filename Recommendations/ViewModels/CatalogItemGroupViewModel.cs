using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class CatalogItemGroupViewModel
    {
        public string GroupName { get; set; }

        public IEnumerable<CatalogItem> CatalogItems { get; set; }

        public PagingPartialViewModel PagingPartialViewModel { get; set; }
    }
}
