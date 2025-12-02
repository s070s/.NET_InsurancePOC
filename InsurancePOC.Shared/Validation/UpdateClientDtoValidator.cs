using FluentValidation;
using InsurancePOC.Shared.Dtos;

namespace InsurancePOC.Shared.Validation
{
    /// <summary>
    /// FluentValidation validator for UpdateClientDto.
    /// Ensures client update data meets business rules before being saved to the database.
    /// Handles edge cases: empty strings, whitespace, invalid email, future DOB, minimum age, phone format.
    /// </summary>
    public class UpdateClientDtoValidator : AbstractValidator<UpdateClientDto>
    {
        private const int MinimumAge = 18;

        public UpdateClientDtoValidator()
        {
            // First Name validation
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .NotNull().WithMessage("First name cannot be null.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("First name cannot be empty or whitespace.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            // Last Name validation
            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .NotNull().WithMessage("Last name cannot be null.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Last name cannot be empty or whitespace.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            // Email validation
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email cannot be null.")
                .Must(email => !string.IsNullOrWhiteSpace(email))
                    .WithMessage("Email cannot be empty or whitespace.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(200).WithMessage("Email cannot exceed 200 characters.");

            // Phone validation (optional but must be valid format if provided)
            When(c => !string.IsNullOrWhiteSpace(c.Phone), () =>
            {
                RuleFor(c => c.Phone)
                    .Matches(@"^\+?[0-9\s\-()]{7,50}$")
                        .WithMessage("Phone number format is invalid. Use digits, spaces, hyphens, parentheses, and optional + prefix.")
                    .MaximumLength(50).WithMessage("Phone number cannot exceed 50 characters.");
            });

            // Date of Birth validation
            RuleFor(c => c.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth cannot be in the future.")
                .Must(dob => dob > DateTime.Today.AddYears(-150))
                    .WithMessage("Date of birth is unrealistic (over 150 years ago).")
                .Must(dob =>
                {
                    var age = CalculateAge(dob);
                    return age >= MinimumAge;
                })
                .WithMessage($"Client must be at least {MinimumAge} years old.");
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
