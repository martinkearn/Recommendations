using BookRecommendations.Models;
using System.Collections.Generic;

namespace BookRecommendations.Interfaces
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetBooks();
    }
}
