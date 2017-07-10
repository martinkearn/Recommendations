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
        private readonly ISkusRepository _skus;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;
        private const int _pageSize = 20; 

        public HomeController(ISkusRepository skusRepository, IRecommendationsRepository recommendationsRepository, ICartRepository cart)
        {
            _skus = skusRepository;
            _recommendations = recommendationsRepository;
            _cart = cart;
        }

        public IActionResult Index(int? page)
        {
            var allSkus = _skus.GetSkus();
     
            //get page of skus
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfSkus = (pageNumber ==1) ?
                allSkus.Take(_pageSize) :
                allSkus.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var totalPages = allSkus.Count() / _pageSize;
            var totalSkus = allSkus.Count();
            var nextPage = pageNumber + 1;
            var previousPage = (pageNumber == 1) ? 1 : pageNumber - 1;

            //construct view model
            var vm = new HomeIndexViewModel()
            {
                Skus = pageOfSkus,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalSkus = totalSkus,
                NextPage = nextPage,
                PreviousPage = previousPage
            };

            //return view
            return View(vm);
        }

        public async Task<IActionResult> Sku(string id)
        {
            //get this book
            var sku = _skus.GetSkuById(id);

            //get ITI and FBT items
            var itiItems = await _recommendations.GetITIItems(id, "5", "0");
            var fbtItems = await _recommendations.GetFBTItems(id, "5", "0");

            //construct view model
            var vm = new HomeSkuViewModel()
            {
                Sku = sku,
                ITIItems = itiItems,
                FBTItems = fbtItems
            };

            //return view
            return View(vm);
        }

        public IActionResult Categories()
        {
            var categories = _skus.GetSkuCategories();

            //construct view model
            var vm = new HomeCategoriesViewModel()
            {
                Categories = categories
            };

            //return view
            return View(vm);
        }

        public IActionResult Category(string id)
        {

            //construct view model
            var vm = new HomeCategoryViewModel()
            {
                 CategoryName = id
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
