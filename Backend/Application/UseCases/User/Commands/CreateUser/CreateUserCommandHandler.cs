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
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Guid.Empty;
            }

            var userEntity = _mapper.Map<Domain.Entities.User>(request.User);

            var createdUserId = await _userRepository.CreateUser(userEntity);

            return createdUserId;
        }
    }
}
