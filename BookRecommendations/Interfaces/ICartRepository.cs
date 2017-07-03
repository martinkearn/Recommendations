using BookRecommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRecommendations.Interfaces
{
    public interface ICartRepository
    {
        Cart GetCart(string user);

        Cart AddToCart(string user, string book, int quantity);

        Cart DeleteFromCart(string user, string book);
    }
}
