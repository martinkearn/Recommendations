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
        private readonly ICartRepository _cart;

        public CartController(ICartRepository cart)
        {
            _cart = cart;
        }

        public IActionResult Index()
        {
            var cart = _cart.CreateGetCart(HttpContext.Session);
            return View(cart);
        }

        public RedirectToActionResult Add(Sku sku, int quantity)
        {
            var cart = _cart.AddToCart(sku, quantity, HttpContext.Session);
            return RedirectToAction("Index");
        }
    }
}