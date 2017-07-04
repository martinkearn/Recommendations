using BookRecommendations.Interfaces;
using BookRecommendations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRecommendations.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppSettings _appSettings;
        private string _cartSessionLabel;

        public CartRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _cartSessionLabel = "cart";
        }

        public Cart CreateGetCart(ISession session)
        {
            var cartData = session.GetString(_cartSessionLabel);

            if (cartData == null)
            {
                //create empty cart
                var cart = new Cart();
                cart.CartItems = new List<CartItem>();

                //serialise to session
                session.SetString(_cartSessionLabel, JsonConvert.SerializeObject(cart));

                //return
                return cart;
            }
            else
            {
                //deserialise and return cart
                return JsonConvert.DeserializeObject<Cart>(cartData);
            }
        }

        public Cart AddToCart(Sku sku, int quantity, ISession session)
        {
            var cart = CreateGetCart(session);

            cart.CartItems.Add(new CartItem() { Sku = sku, Quantity = quantity });

            return cart;
        }

        public Cart DeleteFromCart(Sku sku, ISession session)
        {
            var cart = CreateGetCart(session);

            //find item to remove
            var itemToRemove = cart.CartItems.SingleOrDefault(r => r.Sku.Id == sku.Id);

            //remove it
            if (itemToRemove != null)
                cart.CartItems.Remove(itemToRemove);

            return cart;
        }

    }
}
