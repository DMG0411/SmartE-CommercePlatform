namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Review { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public Product()
        {
        }

        public Product(Guid id, string type, string name, string description, decimal price, int review, Guid userId)
        {
            Id = id;
            Type = type;
            Name = name;
            Description = description;
            Price = price;
            Review = review;
            UserId = userId;
        }
    }
}
