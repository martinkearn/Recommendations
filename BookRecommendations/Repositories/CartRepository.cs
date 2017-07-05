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
                //create and save empty cart
                var cart = new Cart();
                cart.CartItems = new List<CartItem>();
                SaveCart(cart, session);

                //return
                return cart;
            }
            else
            {
                //deserialise and return cart
                var deserializedCart = JsonConvert.DeserializeObject<Cart>(cartData);
                return deserializedCart;
            }
        }

        public Cart AddToCart(Sku sku, int quantity, ISession session)
        {
            var cart = CreateGetCart(session);

            //check if sku is already in cart, if so log the original quantity and remove from cart, refreshing local cart object
            var existingSkusInCart = cart.CartItems.Where(o => o.Sku.Id == sku.Id);
            var originalQuantity = 0;
            foreach (var existingSkuInCart in existingSkusInCart)
            {
                originalQuantity += existingSkuInCart.Quantity;
                cart = DeleteFromCart(existingSkuInCart.Sku, session);
            }

            cart.CartItems.Add(new CartItem() { Sku = sku, Quantity = originalQuantity + quantity });

            SaveCart(cart, session);

            return cart;
        }

        public Cart DeleteFromCart(Sku sku, ISession session)
        {
            var cart = CreateGetCart(session);

            //find items to remove
            var itemToRemove = cart.CartItems.Where(o => o.Sku.Id == sku.Id).FirstOrDefault();

            //remove them
            if (itemToRemove != null)
                cart.CartItems.Remove(itemToRemove);

            SaveCart(cart, session);

            return cart;
        }

        private void SaveCart(Cart cart, ISession session)
        {
            var serializedCart = JsonConvert.SerializeObject(cart);
            session.SetString(_cartSessionLabel, serializedCart);
        }

    }
}
