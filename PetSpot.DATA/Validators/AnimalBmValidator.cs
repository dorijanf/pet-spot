using FluentValidation;
using PetSpot.DATA.Models;

namespace PetSpot.DATA.Validators
{
    public class AnimalBmValidator
        :AbstractValidator<AnimalBm>
    {
        public AnimalBmValidator()
        {
            // Animal name validation
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Animal name is required.");

            // Animal name validation
            RuleFor(x => x.Name).NotNull()
                .WithMessage("Animal name is required");
        }
    }
}
