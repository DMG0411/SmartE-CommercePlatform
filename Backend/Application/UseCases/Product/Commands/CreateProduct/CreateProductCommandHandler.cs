using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return productRepository.CreateProduct(mapper.Map<Domain.Entities.Product>(request.Product));
        }
    }
}
