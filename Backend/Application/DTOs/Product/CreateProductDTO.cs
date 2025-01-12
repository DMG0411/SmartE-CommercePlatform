namespace Application.DTOs.Product
{
    public class CreateProductDTO
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Review { get; set; }

        public Guid UserId { get; set; }

        public CreateProductDTO(string type, string name, string description, decimal price, int review, Guid userId)
        {
            Type = type;
            Name = name;
            Description = description;
            Price = price;
            Review = review;
            UserId = userId;
        }

        public CreateProductDTO()
        {
        }
    }
}
