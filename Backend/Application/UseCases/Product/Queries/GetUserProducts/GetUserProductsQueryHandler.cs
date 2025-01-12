using Application.DTOs.Product;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Product.Queries.GetUserProducts
{
    public class GetUserProductsQueryHandler : IRequestHandler<GetUserProductsQuery, PagedResult<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetUserProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDTO>> Handle(GetUserProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetUserProducts(request.UserId);

            int totalItems = products.Count();

            int skip = request.PageNumber * request.PageSize;
            products = products.Skip(skip).Take(request.PageSize);

            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return new PagedResult<ProductDTO>(productDTOs, totalItems, request.PageNumber, request.PageSize);
        }

    }
}
