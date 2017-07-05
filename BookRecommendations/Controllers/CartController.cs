using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookRecommendations.Interfaces;
using BookRecommendations.Models;

namespace BookRecommendations.Controllers
{
    public class CartController : Controller
    {
        private readonly ISkusRepository _skus;
        private readonly ICartRepository _cart;

        public CartController(ISkusRepository skusRepository, ICartRepository cart)
        {
            _skus = skusRepository;
            _cart = cart;
        }

        public IActionResult Index()
        {
            var cart = _cart.CreateGetCart(HttpContext.Session);
            return View(cart);
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