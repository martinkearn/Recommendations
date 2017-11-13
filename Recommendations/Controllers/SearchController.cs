using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recommendations.ViewModels;
using Recommendations.Models;
using Recommendations.Interfaces;
using Microsoft.Extensions.Options;

namespace Recommendations.Controllers
{
    public class SearchController : Controller
    {

        private readonly ISearchRepository _search;

        public int _pageSize { get; set; }

        public SearchController(IOptions<AppSettings> appSettings, ISearchRepository searchRepository)
        {
            _search = searchRepository;

            _pageSize = Convert.ToInt16(appSettings.Value.ItemsPerPage);
        }

        public async Task<IActionResult> Index(SearchIndexViewModel model)
        {
            //initialise view model data with empty values if passed in data is null
            var query = (model.Query == null) ? string.Empty : model.Query;
            var results = (model.Results == null) ? 
                await _search.SearchCatalogItems(query) : 
                model.Results;

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