using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetProductById(request.Id);
            return mapper.Map<ProductDTO>(product);
        }
    }
}
