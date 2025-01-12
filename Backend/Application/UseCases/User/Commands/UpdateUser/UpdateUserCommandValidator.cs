using FluentValidation;

namespace Application.UseCases.User.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
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

            RuleFor(user => user.User.PhoneNumber)
                .Matches(@"^\+?\d+$")
                .When(user => !string.IsNullOrEmpty(user.User.PhoneNumber))
                .WithMessage("Phone number must contain only digits and may start with '+' sign");
        }
    }
}
