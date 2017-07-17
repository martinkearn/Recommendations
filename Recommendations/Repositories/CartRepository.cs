using Recommendations.Interfaces;
using Recommendations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Repositories
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

        public Cart AddToCart(CatalogItem catalogItem, int quantity, ISession session)
        {
            var cart = CreateGetCart(session);

            //check if catalogItem is already in cart, if so log the original quantity and remove from cart, refreshing local cart object
            var existingcatalogItemsInCart = cart.CartItems.Where(o => o.CatalogItem.Id == catalogItem.Id);
            var originalQuantity = 0;
            foreach (var existingcatalogItemInCart in existingcatalogItemsInCart)
            {
                originalQuantity += existingcatalogItemInCart.Quantity;
                cart = DeleteFromCart(existingcatalogItemInCart.CatalogItem, session);
            }

            cart.CartItems.Add(new CartItem() { CatalogItem = catalogItem, Quantity = originalQuantity + quantity });

            //update totals
            cart = UpdateCartTotals(cart);

            SaveCart(cart, session);

            return cart;
        }

        public Cart DeleteFromCart(CatalogItem catalogItem, ISession session)
        {
            var cart = CreateGetCart(session);

            //find items to remove
            var itemToRemove = cart.CartItems.Where(o => o.CatalogItem.Id == catalogItem.Id).FirstOrDefault();

            //remove them
            if (itemToRemove != null)
                cart.CartItems.Remove(itemToRemove);

            //update totals
            cart = UpdateCartTotals(cart);

            SaveCart(cart, session);

            return cart;
        }

        private void SaveCart(Cart cart, ISession session)
        {
            var serializedCart = JsonConvert.SerializeObject(cart);
            session.SetString(_cartSessionLabel, serializedCart);
        }

        private Cart UpdateCartTotals(Cart cart)
        {
            var totalQuantity = 0;
            decimal totalPrice = 0;

            foreach (var cartItem in cart.CartItems)
            {
                totalQuantity += cartItem.Quantity;
                totalPrice += cartItem.CatalogItem.Price * cartItem.Quantity;
            }

            cart.TotalQuantity = totalQuantity;
            cart.TotalPrice = totalPrice;

            return cart;
        }

    }
}
