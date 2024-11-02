using Application.DTOs;
using Application.UseCases.Commands;
using Application.UseCases.Queries;
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
        public async Task<ActionResult<Guid>> CreateProduct(CreateProductCommand command)
        {
            var taskId = await mediator.Send(command);
            return Created(nameof(ProductDTO), taskId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetTaskById(Guid id)
        {
            return await mediator.Send(new GetProductByIdQuery(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTask(ProductDTO task)
        {
            await mediator.Send(new UpdateProductCommand(task));
            return Ok(task);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTasks()
        {
            var tasks = await mediator.Send(new GetAllProductsQuery());
            return Ok(tasks);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            await mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
    }
}
