using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRecommendations.Models
{
    public class Cart
    {
        public string User { get; set; }
        public List<Book> Books { get; set; }
    }
}
