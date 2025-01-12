using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Guid> CreateProduct(Product product);
        Task<Product> GetProductById(Guid id);
        Task UpdateProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetFilteredProducts(string? type, decimal? minPrice, decimal? maxPrice, int? minReview);
        Task DeleteProduct(Guid id);

        Task<IEnumerable<Product>> GetUserProducts(Guid userId);
    }
}
