namespace Application.DTOs
{
    public class CreateProductDTO
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Review { get; set; }

        public CreateProductDTO(string type, string name, string description, double price, int review)
        {
            Type = type;
            Name = name;
            Description = description;
            Price = price;
            Review = review;
        }
    }
}
