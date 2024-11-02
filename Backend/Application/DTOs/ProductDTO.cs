namespace Application.DTOs
{
    public class ProductDTO : CreateProductDTO
    {
        public Guid? Id { get; set; }

        public ProductDTO(): base()
        {
        }

        public ProductDTO(Guid id, string type, string name, string description, double price, int review) : base(type, name, description, price, review)
        {
            Id = id;
        }
    }
}
