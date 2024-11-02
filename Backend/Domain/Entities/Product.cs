namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Review { get; set; }

        public Product()
        {
        }

        public Product(Guid id, string type, string name, string description, double price, int review)
        {
            Id = id;
            Type = type;
            Name = name;
            Description = description;
            Price = price;
            Review = review;
        }
    }
}
