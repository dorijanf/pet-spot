using FluentValidation;
using PetSpot.DATA.Models;
using System.Linq;

namespace PetSpot.DATA.Validators
{
    public class RegisterUserBmValidator
        : AbstractValidator<RegisterUserBm>
    {
        public RegisterUserBmValidator()
        {
            // UserName validation
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("Username is required.");

            RuleFor(x => x.UserName).NotNull()
                .WithMessage("Username is required.");

            RuleFor(x => x.UserName).MaximumLength(32)
                .WithMessage("Username can have a maximum of 32 characters");

            RuleFor(x => x.UserName).MinimumLength(6)
                .WithMessage("Username must have a minimum of 6 characters");

            // Email
            RuleFor(x => x.Email).EmailAddress()
                .WithMessage("Email must be in a correct format.");

            RuleFor(x => x.Email).NotNull()
                .WithMessage("Email is required.");

            // Password validation
            RuleFor(x => x.Password).NotNull()
                .WithMessage("Password is required.");

            RuleFor(x => x.Password).MinimumLength(10)
                .WithMessage("Password must have a minimum of 8 characters.");

            RuleFor(x => x.Password).Must(ContainLowercase)
                .WithMessage("Password must contain at least one lowercase letter.");

            RuleFor(x => x.Password).Must(ContainUppercase)
                .WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.Password).Must(ContainNumber)
                .WithMessage("Password must contain at least one number.");

            RuleFor(x => x.Password).Must(ContainSpecial)
                .WithMessage("Password must contain at least one special character.");

        }

        private bool ContainLowercase(string text)
        {
            return text.Any(ch => char.IsLower(ch));
        }

        private bool ContainUppercase(string text)
        {
            return text.Any(ch => char.IsUpper(ch));
        }

        private bool ContainNumber(string text)
        {
            return text.Any(ch => char.IsDigit(ch));
        }

        private bool ContainSpecial(string text)
        {
            return text.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }
}
