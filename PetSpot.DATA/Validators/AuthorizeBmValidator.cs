using FluentValidation;
using PetSpot.DATA.Models;

namespace PetSpot.DATA.Validators
{
    public class AuthorizeBmValidator
        : AbstractValidator<AuthorizeBm>
    {
        public AuthorizeBmValidator()
        {
            // UserName validation
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("Username is required.");

            RuleFor(x => x.UserName).NotNull()
                .WithMessage("Username is required.");

            // Password validation
            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password is required.");

            RuleFor(x => x.Password).NotNull()
                .WithMessage("Password is required.");
        }
    }
}
