using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateProduct(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return product.Id;
        }

        public async Task DeleteProduct(Guid id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                throw new ResourceNotFoundException(ExceptionsResource.NoResourceFound);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFilteredProducts(string? type, decimal? minPrice, decimal? maxPrice, int? minReview)
        {
            var query = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.Type == type);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (minReview.HasValue)
                query = query.Where(p => p.Review >= minReview);


            return await query.ToListAsync();
        }

        public Task<Product> GetProductById(Guid id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                throw new ResourceNotFoundException(ExceptionsResource.NoResourceFound);
            }
            return Task.FromResult(product);
        }

        public async Task UpdateProduct(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetUserProducts(Guid userId)
        {
            return await context.Products.Where(p => p.UserId == userId).ToListAsync();
        }

    }
}
