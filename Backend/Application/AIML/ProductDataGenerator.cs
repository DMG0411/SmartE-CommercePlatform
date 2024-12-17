using System;
using System.Collections.Generic;

namespace Application.AIML
{
    public static class ProductDataGenerator
    {
        public static List<ProductData> GetProducts()
        {
            return new List<ProductData>
            {
                new() { Type = "Electronics", Name = "Smartphone", Description = "Flagship phone", Price = 999.99f, Review = 5 },
                new() { Type = "Electronics", Name = "Laptop", Description = "High-performance laptop", Price = 1200.50f, Review = 5 },
                new() { Type = "Electronics", Name = "Headphones", Description = "Noise-cancelling headphones", Price = 299.99f, Review = 4 },
                new() { Type = "Electronics", Name = "Smartwatch", Description = "Fitness tracking smartwatch", Price = 199.99f, Review = 4 },
                new() { Type = "Appliances", Name = "Microwave", Description = "Compact microwave", Price = 150.50f, Review = 4 },
                new() { Type = "Appliances", Name = "Blender", Description = "High-speed blender", Price = 80.00f, Review = 3 },
                new() { Type = "Appliances", Name = "Washing Machine", Description = "Front-load washing machine", Price = 500.00f, Review = 5 },
                new() { Type = "Appliances", Name = "Coffee Maker", Description = "Automatic coffee machine", Price = 120.00f, Review = 5 },
                new() { Type = "Furniture", Name = "Office Chair", Description = "Ergonomic office chair", Price = 200.00f, Review = 4 },
                new() { Type = "Furniture", Name = "Dining Table", Description = "Wooden dining table", Price = 300.00f, Review = 3 },
                new() { Type = "Furniture", Name = "Sofa", Description = "Comfortable living room sofa", Price = 700.00f, Review = 4 },
                new() { Type = "Furniture", Name = "Bookshelf", Description = "Wooden bookshelf", Price = 100.00f, Review = 5 },
                new() { Type = "Furniture", Name = "Desk", Description = "Adjustable office desk", Price = 250.00f, Review = 4 },
                new() { Type = "Clothing", Name = "T-shirt", Description = "Cotton T-shirt", Price = 20.00f, Review = 5 },
                new() { Type = "Clothing", Name = "Jeans", Description = "Denim jeans", Price = 50.00f, Review = 4 },
                new() { Type = "Clothing", Name = "Jacket", Description = "Winter jacket", Price = 120.00f, Review = 5 },
                new() { Type = "Clothing", Name = "Sweater", Description = "Wool sweater", Price = 40.00f, Review = 4 },
                new() { Type = "Clothing", Name = "Shoes", Description = "Running shoes", Price = 60.00f, Review = 4 },
                new() { Type = "Toys", Name = "Action Figure", Description = "Superhero action figure", Price = 25.00f, Review = 4 },
                new() { Type = "Toys", Name = "Board Game", Description = "Family board game", Price = 30.00f, Review = 5 },
                new() { Type = "Toys", Name = "Puzzle", Description = "500-piece puzzle", Price = 15.00f, Review = 3 },
                new() { Type = "Sports", Name = "Tennis Racket", Description = "Professional tennis racket", Price = 150.00f, Review = 4 },
                new() { Type = "Sports", Name = "Football", Description = "Official football", Price = 40.00f, Review = 5 },
                new() { Type = "Sports", Name = "Yoga Mat", Description = "Non-slip yoga mat", Price = 25.00f, Review = 4 },
                new() { Type = "Sports", Name = "Dumbbells", Description = "Set of adjustable dumbbells", Price = 80.00f, Review = 5 },
                new() { Type = "Books", Name = "Novel", Description = "Fiction novel", Price = 15.00f, Review = 4 },
                new() { Type = "Books", Name = "Cookbook", Description = "Healthy recipe cookbook", Price = 20.00f, Review = 5 },
                new() { Type = "Books", Name = "Textbook", Description = "Computer science textbook", Price = 50.00f, Review = 3 },
                new() { Type = "Books", Name = "Biography", Description = "Biography of a famous person", Price = 18.00f, Review = 4 }
            };
        }
    }
}
