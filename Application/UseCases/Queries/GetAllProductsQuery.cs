using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>>
    {
        public GetAllProductsQuery() { }
    }
}
