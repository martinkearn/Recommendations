using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookRecommendations.ViewModels
{
    public class HomeIndexViewModel
    {
        public SelectList Books { get; set; }
    }
}
