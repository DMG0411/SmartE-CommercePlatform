using Application.DTOs;
using Application.UseCases.Product.Commands.CreateProduct;
using Application.UseCases.Product.Commands.DeleteProduct;
using Application.UseCases.Product.Commands.UpdateProduct;
using Application.UseCases.Product.Queries.GetAllProducts;
using Application.UseCases.Product.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers
{
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProduct(CreateProductDTO product)
        {
            var productId = await mediator.Send(new CreateProductCommand(product));
            return Created(nameof(ProductDTO), productId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(Guid id)
        {
            return await mediator.Send(new GetProductByIdQuery(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductDTO product)
        {
            await mediator.Send(new UpdateProductCommand(product));
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
    }
}
