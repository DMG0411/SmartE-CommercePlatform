using Application.DTOs.Product;
using MediatR;

namespace Application.UseCases.Product.Queries.GetUserProducts
{
    public class GetUserProductsQuery : IRequest<PagedResult<ProductDTO>>
    {
        public Guid UserId { get; set; }

        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public GetUserProductsQuery(Guid userId, int pageNumber, int pageSize)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
