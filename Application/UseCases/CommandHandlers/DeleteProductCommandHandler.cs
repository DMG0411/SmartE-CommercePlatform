using Application.UseCases.Commands;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
           await productRepository.DeleteProduct(request.Id);
        }
    }
}
