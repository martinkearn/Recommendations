﻿namespace BookRecommendations.Models
{
    public class Sku
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Year { get; set; }
        public decimal Price { get; set; }
    }
}