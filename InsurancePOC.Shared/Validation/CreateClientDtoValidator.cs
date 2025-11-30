using FluentValidation;
using InsurancePOC.Shared.Dtos;

namespace InsurancePOC.Shared.Validation
{
    /// <summary>
    /// FluentValidation validator for CreateClientDto.
    /// Ensures client data meets business rules before being saved to the database.
    /// </summary>
    public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
    {
        public CreateClientDtoValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(200);
            RuleFor(c => c.Phone).MaximumLength(50);
            // Date of birth must be in the past (cannot create client born in the future)
            RuleFor(c => c.DateOfBirth).LessThan(DateTime.Today);
        }
    }
}
