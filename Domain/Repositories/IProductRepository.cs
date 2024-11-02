using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Guid> CreateProduct(Product product);
        Task<Product> GetProductById(Guid id);
        Task UpdateProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
        Task DeleteProduct(Guid id);
    }
}
