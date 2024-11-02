using Application.DTOs;
using MediatR;

namespace Application.UseCases.Product.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public CreateProductDTO Product { get; set; }

        public CreateProductCommand(CreateProductDTO product)
        {
            Product = product;
        }
    }
}
