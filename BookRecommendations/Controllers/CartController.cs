using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookRecommendations.Interfaces;
using BookRecommendations.Models;
using BookRecommendations.ViewModels;

namespace BookRecommendations.Controllers
{
    public class CartController : Controller
    {
        private readonly ISkusRepository _skus;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;

        public CartController(ISkusRepository skusRepository, IRecommendationsRepository recommendationsRepository, ICartRepository cart)
        {
            _skus = skusRepository;
            _recommendations = recommendationsRepository;
            _cart = cart;
        }

        public async Task<IActionResult> Index()
        {
            var cart = _cart.CreateGetCart(HttpContext.Session);

            var ids = string.Join(",", cart.CartItems.Select(o => o.Sku.Id));
            var itiItems = await _recommendations.GetITIItems(ids, "5", "0");

            //construct view model
            var vm = new CartIndexViewModel()
            {
                Cart = cart,
                ITIItems = itiItems
            };

            //return view
            return View(vm);
        }

        public RedirectToActionResult Add(string id)
        {
            var sku = _skus.GetSkuById(id);
            var cart = _cart.AddToCart(sku, 1, HttpContext.Session);
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Remove(string id)
        {
            var sku = _skus.GetSkuById(id);
            var cart = _cart.DeleteFromCart(sku, HttpContext.Session);
            return RedirectToAction("Index");
        }
    }
}