using AutoMapper;
using FluentValidation;
using MediatR;
using Domain.Repositories;
using Domain.Entities; // Ensure this using directive is present
using Application.DTOs.Product;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductCommand> _validator;

        public CreateProductCommandHandler(
            IProductRepository repository,
            IMapper mapper,
            IValidator<CreateProductCommand> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Optionally, log the validation failures here
                // For example: _logger.LogWarning("Invalid CreateProductCommand: {Errors}", validationResult.Errors);

                // Return Guid.Empty to indicate failure
                return Guid.Empty;
            }

            // Map DTO to domain entity
            var productEntity = _mapper.Map<Domain.Entities.Product>(request.Product);

            // Add the product to the repository and get the generated Guid
            var createdProductId = await _repository.CreateProduct(productEntity);

            return createdProductId;
        }
    }
}