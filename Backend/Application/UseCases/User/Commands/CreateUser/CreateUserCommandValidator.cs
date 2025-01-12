using FluentValidation;

namespace Application.UseCases.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(user => user.User.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(30).WithMessage("Username must be less than 30 characters long");

            RuleFor(user => user.User.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(10).WithMessage("Password must be at least 10 characters long");

            RuleFor(user => user.User.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be valid");

            
        }
    }
}
