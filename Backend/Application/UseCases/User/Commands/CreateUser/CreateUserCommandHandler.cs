using AutoMapper;
using FluentValidation;
using MediatR;
using Domain.Repositories;
using Domain.Entities;
using Application.DTOs.User;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IValidator<CreateUserCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Optionally, log the validation failures here
                // For example: _logger.LogWarning("Invalid CreateUserCommand: {Errors}", validationResult.Errors);

                // Return Guid.Empty to indicate failure
                return Guid.Empty;
            }

            // Map DTO to domain entity
            var userEntity = _mapper.Map<Domain.Entities.User>(request.User);

            // Add the user to the repository and get the generated Guid
            var createdUserId = await _userRepository.CreateUser(userEntity);

            return createdUserId;
        }
    }
}
