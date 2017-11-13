using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recommendations.ViewModels;
using Recommendations.Models;

namespace Recommendations.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index(SearchIndexViewModel model)
        {
            //initialise view model data with empty values if passed in data is null
            var query = (model.Query == null) ? string.Empty : model.Query;
            var results = (model.Results == null) ? new List<CatalogItem>() : model.Results;

            //construct view model
            var vm = new SearchIndexViewModel()
            {
                Query = query,
                Results = results
            };

            //return view
            return View(vm);
        }



    }
}