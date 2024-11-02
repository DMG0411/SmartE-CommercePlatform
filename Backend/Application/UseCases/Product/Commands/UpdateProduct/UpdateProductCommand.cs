using Application.DTOs;
using MediatR;

namespace Application.UseCases.Product.Commands.UpdateProduct
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
