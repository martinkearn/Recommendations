using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recommendations.Interfaces;
using Recommendations.ViewModels;
using Recommendations.Models;

namespace Recommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICatalogRepository _catalogItems;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;
        private const int _pageSize = 21; 

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
            var vm = new CatalogItemGroupViewModel()
            {
                GroupName = "Home",
                CatalogItems = pageOfcatalogItems,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
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

            //get recommendations
            var recommendations = await _recommendations.GetRecommendations(new List<CatalogItem>() { catalogItem }, "100", "0");

            //get body section
            var bodySection = _catalogItems.GetOutfitSection(catalogItem);

            //construct view model
            var vm = new CatalogItemViewModel()
            {
                CatalogItem = catalogItem,
                Recommendations = recommendations,
                OutfitSection = bodySection
            };

            //return view
            return View(vm);
        }

        public IActionResult Categories()
        {
            var categories = _catalogItems.GetCategories();

            //construct view model
            var vm = new CategoriesViewModel()
            {
                Categories = categories
            };

            //return view
            return View(vm);
        }

        public IActionResult Category(string id, int? page)
        {
            var category = _catalogItems.GetCategories().Where(o => o.Title.ToLower() == id.ToLower()).FirstOrDefault();

            var allCatalogItems = _catalogItems.GetCatalogItems().Where(o => o.Type.ToLower() == id.ToLower());

            //get page of catalogItems
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfcatalogItems = (pageNumber == 1) ?
                allCatalogItems.Take(_pageSize) :
                allCatalogItems.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var totalPages = allCatalogItems.Count() / _pageSize;
            var totalcatalogItems = allCatalogItems.Count();
            var nextPage = pageNumber + 1;
            var previousPage = (pageNumber == 1) ? 1 : pageNumber - 1;

            //construct view model
            var vm = new CategoryViewModel()
            {
                GroupName = id,
                CatalogItems = pageOfcatalogItems,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalcatalogItems = totalcatalogItems,
                NextPage = nextPage,
                PreviousPage = previousPage,
                RelatedCategoryTitles = category.TopRelatedCategoryTitles,
                OutfitSection = category.OutfitSection
            };

            //return view
            return View(vm);
        }

        public IActionResult Brands()
        {
            var brands = _catalogItems.GetBrands();

            //construct view model
            var vm = new BrandsViewModel()
            {
                Brands = brands
            };

            //return view
            return View(vm);
        }

        public IActionResult Brand(string id, int? page)
        {
            var allCatalogItems = _catalogItems.GetCatalogItems().Where(o => o.Brand.ToLower() == id.ToLower());

            //get page of catalogItems
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfcatalogItems = (pageNumber == 1) ?
                allCatalogItems.Take(_pageSize) :
                allCatalogItems.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var totalPages = allCatalogItems.Count() / _pageSize;
            var totalcatalogItems = allCatalogItems.Count();
            var nextPage = pageNumber + 1;
            var previousPage = (pageNumber == 1) ? 1 : pageNumber - 1;

            //construct view model
            var vm = new CatalogItemGroupViewModel()
            {
                GroupName = id,
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
