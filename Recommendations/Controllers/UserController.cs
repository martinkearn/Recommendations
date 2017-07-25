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
    public class UserController : Controller
    {
        private readonly ICatalogRepository _catalog;
        private readonly IRecommendationsRepository _recommendations;
        private readonly IUserRepository _user;

        public UserController(ICatalogRepository catalogRepository, IRecommendationsRepository recommendationsRepository, IUserRepository user)
        {
            _catalog = catalogRepository;
            _recommendations = recommendationsRepository;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _user.GetUser(HttpContext.Session);

            //get personalised recommendations
            var recommnedations = await _recommendations.GetPersonalizedRecommendedItemsByUser(userId, "5");

            //construct view model
            var vm = new UserLoginViewModel()
            {
                UserId = userId,
                Recommendations = recommnedations
            };

            //return view
            return View(vm);
        }


        [HttpPost]
        public IActionResult Login(UserLoginViewModel model)
        {
            _user.SetUser(HttpContext.Session, model.UserId);
            return RedirectToAction("Index");
        }
    }
}