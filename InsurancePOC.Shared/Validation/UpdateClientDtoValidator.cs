using FluentValidation;
using InsurancePOC.Shared.Dtos;

namespace InsurancePOC.Shared.Validation
{
    /// <summary>
    /// FluentValidation validator for UpdateClientDto.
    /// Ensures client update data meets business rules before being saved to the database.
    /// </summary>
    public class UpdateClientDtoValidator : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientDtoValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(200);
            RuleFor(c => c.Phone).MaximumLength(50);
            // Date of birth must be in the past
            RuleFor(c => c.DateOfBirth).LessThan(DateTime.Today);
        }
    }
}
