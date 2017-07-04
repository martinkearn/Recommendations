using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookRecommendations.Interfaces;
using BookRecommendations.ViewModels;

namespace BookRecommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISkusRepository _skus;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;

        public HomeController(ISkusRepository skusRepository, IRecommendationsRepository recommendationsRepository, ICartRepository cart)
        {
            _skus = skusRepository;
            _recommendations = recommendationsRepository;
            _cart = cart;
        }

        public IActionResult Index()
        {
            var allBooks = _skus.GetSkus();
            return View(allBooks);
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
