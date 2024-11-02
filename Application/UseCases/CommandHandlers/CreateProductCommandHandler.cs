using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
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
            return productRepository.CreateProduct(new Product(request.Id,request.Type, request.Name, request.Description, request.Price, request.Review));
        }
    }
}
