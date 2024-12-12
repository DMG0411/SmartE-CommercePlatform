using Application.DTOs.Cart;
using AutoMapper;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Cart.Queries.GetCartById
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, CartDTO>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCartByIdQuery> _validator;

        public GetCartByIdQueryHandler(ICartRepository cartRepository, IMapper mapper, IValidator<GetCartByIdQuery> validator)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CartDTO> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Invalid GetCartByIdQuery: {errors}");
            }

            var cart = await _cartRepository.GetCartById(request.UserId);
            return _mapper.Map<CartDTO>(cart);
        }
    }
}
