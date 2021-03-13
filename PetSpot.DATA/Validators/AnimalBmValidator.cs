using FluentValidation;
using PetSpot.DATA.Models;

namespace PetSpot.DATA.Validators
{
    public class AnimalBmValidator
        :AbstractValidator<AnimalBm>
    {
        public AnimalBmValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Animal name is required.");

            RuleFor(x => x.Name).NotNull()
                .WithMessage("Animal name is required");
        }
    }
}
