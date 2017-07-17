using Recommendations.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface ICartRepository
    {
        Cart CreateGetCart(ISession session);

        Cart AddToCart(CatalogItem catalogItem, int quantity, ISession session);

        Cart DeleteFromCart(CatalogItem catalogItem, ISession session);
    }
}
