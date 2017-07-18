using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recommendations.Interfaces;
using Recommendations.ViewModels;

namespace Recommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICatalogRepository _catalogItems;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;
        private const int _pageSize = 20; 

        public HomeController(ICatalogRepository catalogItemsRepository, IRecommendationsRepository recommendationsRepository, ICartRepository cart)
        {
            _catalogItems = catalogItemsRepository;
            _recommendations = recommendationsRepository;
            _cart = cart;
        }

        public IActionResult Index(int? page)
        {
            var allcatalogItems = _catalogItems.GetCatalogItems();
     
            //get page of catalogItems
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfcatalogItems = (pageNumber ==1) ?
                allcatalogItems.Take(_pageSize) :
                allcatalogItems.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var totalPages = allcatalogItems.Count() / _pageSize;
            var totalcatalogItems = allcatalogItems.Count();
            var nextPage = pageNumber + 1;
            var previousPage = (pageNumber == 1) ? 1 : pageNumber - 1;

            //construct view model
            var vm = new HomeIndexViewModel()
            {
                CatalogItems = pageOfcatalogItems,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalcatalogItems = totalcatalogItems,
                NextPage = nextPage,
                PreviousPage = previousPage
            };

            //return view
            return View(vm);
        }

        public async Task<IActionResult> CatalogItem(string id)
        {
            //get this catalogItem
            var catalogItem = _catalogItems.GetCatalogItemById(id);

            //get ITI and FBT items
            var itiItems = await _recommendations.GetITIItems(id, "5", "0");

            //construct view model
            var vm = new HomeCatalogItemViewModel()
            {
                CatalogItem = catalogItem,
                ITIItems = itiItems
            };

            //return view
            return View(vm);
        }

        public IActionResult Categories()
        {
            var categories = _catalogItems.GetCatalogItemCategories();

            //construct view model
            var vm = new HomeCategoriesViewModel()
            {
                Categories = categories
            };

            //return view
            return View(vm);
        }

        public IActionResult Category(string id, int? page)
        {
            var allcatalogItems = _catalogItems.GetCatalogItems().Where(o => o.Type.ToLower() == id.ToLower());

            //get page of catalogItems
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfcatalogItems = (pageNumber == 1) ?
                allcatalogItems.Take(_pageSize) :
                allcatalogItems.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var totalPages = allcatalogItems.Count() / _pageSize;
            var totalcatalogItems = allcatalogItems.Count();
            var nextPage = pageNumber + 1;
            var previousPage = (pageNumber == 1) ? 1 : pageNumber - 1;

            //construct view model
            var vm = new HomeCategoryViewModel()
            {
                CategoryName = id,
                CatalogItems = pageOfcatalogItems,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalcatalogItems = totalcatalogItems,
                NextPage = nextPage,
                PreviousPage = previousPage
            };

            //return view
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
