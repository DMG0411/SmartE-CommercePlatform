using Application.AIML;
using Application.DTOs.Product;
using Application.UseCases.Product.Commands.CreateProduct;
using Application.UseCases.Product.Commands.DeleteProduct;
using Application.UseCases.Product.Commands.UpdateProduct;
using Application.UseCases.Product.Queries.GetAllProducts;
using Application.UseCases.Product.Queries.GetProductById;
using Application.UseCases.Product.Queries.GetUserProducts;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProductPricePredictionModel _predictionModel;

        public ProductController(IMediator mediator, ProductPricePredictionModel predictionModel)
        {
            _mediator = mediator;
            _predictionModel = predictionModel;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProduct(CreateProductDTO product)
        {
            var productId = await _mediator.Send(new CreateProductCommand(product));
            return Created(nameof(ProductDTO), productId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(Guid id)
        {
            return await _mediator.Send(new GetProductByIdQuery(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductDTO product)
        {
            await _mediator.Send(new UpdateProductCommand(product));
            return NoContent();
        }

        [HttpGet("my-products")]
        public async Task<ActionResult<PagedResult<ProductDTO>>> GetUserProducts(
         [FromQuery] int pageNumber = 0,
         [FromQuery] int pageSize = 10
        )
        {
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value!);
            var products = await _mediator.Send(new GetUserProductsQuery(userId, pageNumber, pageSize));

            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts(
         [FromQuery] string? type,
         [FromQuery] decimal? minPrice,
         [FromQuery] decimal? maxPrice,
         [FromQuery] int? minReview,
         [FromQuery] int pageNumber = 0,
         [FromQuery] int pageSize = 10)
        {
            var query = new GetAllProductsQuery(type, minPrice, maxPrice, minReview, pageNumber, pageSize);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
    }
}
