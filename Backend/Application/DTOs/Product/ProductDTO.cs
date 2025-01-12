namespace Application.DTOs.Product
{
    public class ProductDTO : CreateProductDTO
    {
        public Guid? Id { get; set; }

        public ProductDTO() : base()
        {
        }

        public ProductDTO(Guid id, string type, string name, string description, decimal price, int review, Guid userId) : base(type, name, description, price, review, userId)
        {
            Id = id;
        }
    }
}
