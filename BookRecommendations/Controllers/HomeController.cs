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
        private readonly IBooksRepository _books;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;

        public HomeController(IBooksRepository booksRepository, IRecommendationsRepository recommendationsRepository, ICartRepository cart)
        {
            _books = booksRepository;
            _recommendations = recommendationsRepository;
            _cart = cart;
        }

        public IActionResult Index()
        {
            var allBooks = _books.GetBooks();
            return View(allBooks);
        }

        public IActionResult Cart()
        {
            var cart = _cart.GetCart("martin");
            return View(cart);
        }

        public async Task<IActionResult> Book(string id)
        {
            //get this book
            var book = _books.GetBookById(id);

            //get ITI and FBT items
            var itiItems = await _recommendations.GetITIItems(id, "5", "0");
            var fbtItems = await _recommendations.GetFBTItems(id, "5", "0");

            //construct view model
            var vm = new HomeBookViewModel()
            {
                Book = book,
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
