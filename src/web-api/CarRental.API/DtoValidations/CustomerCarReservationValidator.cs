using CarRental.Application.Dtos;
using FluentValidation;

namespace CarRental.API.DtoValidations
{
    public class CustomerCarReservationValidator : AbstractValidator<CarReservationDto>
    {
        public CustomerCarReservationValidator()
        {
            RuleFor(p => p.CarId)
                .NotEmpty()
                .Must(ValidateGuidValue)
                .WithMessage("Car id is not provided in the valid format. It should be a valid GUID value.");

            RuleFor(p => p.RentFrom).Must(date => date != default);

            RuleFor(p => p.RentTo).Must(date => date != default);
        }

        private bool ValidateGuidValue(string stringValue)
        {
            return Guid.TryParse(stringValue, out var result);
        }
    }
}
