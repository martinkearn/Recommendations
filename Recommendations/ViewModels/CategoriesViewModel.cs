using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class CategoriesViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
