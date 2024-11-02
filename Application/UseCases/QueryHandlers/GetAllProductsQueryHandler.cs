using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetAllProducts();
            return mapper.Map<IEnumerable<ProductDTO>>(products);
        }
    }
}
