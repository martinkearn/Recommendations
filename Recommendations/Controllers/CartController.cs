using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recommendations.Interfaces;
using Recommendations.Models;
using Recommendations.ViewModels;

namespace Recommendations.Controllers
{
    public class CartController : Controller
    {
        private readonly ICatalogRepository _catalogItems;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;

        public CartController(ICatalogRepository catalogItemsRepository, IRecommendationsRepository recommendationsRepository, ICartRepository cart)
        {
            _catalogItems = catalogItemsRepository;
            _recommendations = recommendationsRepository;
            _cart = cart;
        }

        public async Task<IActionResult> Index()
        {
            var cart = _cart.CreateGetCart(HttpContext.Session);

            var cartCatalogItems = cart.CartItems.Select(o => o.CatalogItem).ToList();
            
            var recommendations = await _recommendations.GetRecommendations(cartCatalogItems, "100", "0");

            //construct view model
            var vm = new CartIndexViewModel()
            {
                Cart = cart,
                Recommendations = recommendations
            };

            //return view
            return View(vm);
        }

        public RedirectToActionResult Add(string id)
        {
            var catalogItem = _catalogItems.GetCatalogItemById(id);
            var cart = _cart.AddToCart(catalogItem, 1, HttpContext.Session);
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Remove(string id)
        {
            var catalogItem = _catalogItems.GetCatalogItemById(id);
            var cart = _cart.DeleteFromCart(catalogItem, HttpContext.Session);
            return RedirectToAction("Index");
        }
    }
}