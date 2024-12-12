// Application/UseCases/Product/Queries/GetAllProducts/GetAllProductsQueryHandler.cs
using Application.DTOs.Product;
using AutoMapper;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Product.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAllProductsQuery> _validator;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper, IValidator<GetAllProductsQuery> validator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<PagedResult<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Validate the query
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Invalid GetAllProductsQuery: {errors}");
            }

            var products = await _productRepository.GetAllProducts();

            if (!string.IsNullOrEmpty(request.Type))
                products = products.Where(p => p.Type == request.Type);

            if (request.MinPrice.HasValue)
                products = products.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.MinReview.HasValue)
                products = products.Where(p => p.Review >= request.MinReview.Value);

            int totalItems = products.Count();

            int skip = request.PageNumber * request.PageSize;
            products = products.Skip(skip).Take(request.PageSize);

            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return new PagedResult<ProductDTO>(productDTOs, totalItems, request.PageNumber, request.PageSize);
        }
    }
}
