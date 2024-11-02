using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public Guid Id { get; set; }

        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
