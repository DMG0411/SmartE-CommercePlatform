using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Product.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await productRepository.UpdateProduct(mapper.Map<Domain.Entities.Product>(request.Product));
        }

    }
}
