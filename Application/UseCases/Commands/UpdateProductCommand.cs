using Application.DTOs;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateProductCommand : IRequest
    {
        public ProductDTO Product { get; set; }

        public UpdateProductCommand(ProductDTO product)
        {
            Product = product;
        }
    }
}
