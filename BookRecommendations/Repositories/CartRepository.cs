using BookRecommendations.Interfaces;
using BookRecommendations.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRecommendations.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppSettings _appSettings;
        private List<Cart> _carts;

        public CartRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _carts = new List<Cart>();
        }

        public Cart GetCart(string user)
        {
            var cart = _carts.Where(o => o.User == user).FirstOrDefault();

            if (cart == null)
            {
                var newCart = new Cart()
                {
                    User = user,
                    Books = new List<Book>()
                };

                _carts.Add(newCart);

                cart = newCart;
            }

            return cart;
        }

        public Cart AddToCart(string user, string book, int quantity)
        {
            var cart = new Cart();

            return cart;
        }

        public Cart DeleteFromCart(string user, string book)
        {
            var cart = new Cart();

            return cart;
        }
    }
}
