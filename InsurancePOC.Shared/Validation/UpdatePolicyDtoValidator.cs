using FluentValidation;
using InsurancePOC.Shared.Dtos;

namespace InsurancePOC.Shared.Validation
{
    /// <summary>
    /// FluentValidation validator for UpdatePolicyDto.
    /// Ensures policy update data meets business rules before being saved to the database.
    /// Handles edge cases: invalid policy numbers, negative premiums, invalid dates,
    /// precision validation, date ranges, policy type validation, activation logic.
    /// </summary>
    public class UpdatePolicyDtoValidator : AbstractValidator<UpdatePolicyDto>
    {
        private static readonly string[] ValidPolicyTypes = new[]
        {
            "Health", "Car", "Home", "Life", "Travel", "Property", "Liability"
        };

        public UpdatePolicyDtoValidator()
        {
            // Policy Number validation
            RuleFor(p => p.PolicyNumber)
                .NotEmpty().WithMessage("Policy number is required.")
                .NotNull().WithMessage("Policy number cannot be null.")
                .Must(num => !string.IsNullOrWhiteSpace(num))
                    .WithMessage("Policy number cannot be empty or whitespace.")
                .Matches(@"^[A-Z0-9\-]{5,50}$")
                    .WithMessage("Policy number format is invalid. Use uppercase letters, digits, and hyphens (5-50 characters).")
                .MaximumLength(50).WithMessage("Policy number cannot exceed 50 characters.");

            // Policy Type validation
            RuleFor(p => p.PolicyType)
                .NotEmpty().WithMessage("Policy type is required.")
                .NotNull().WithMessage("Policy type cannot be null.")
                .Must(type => !string.IsNullOrWhiteSpace(type))
                    .WithMessage("Policy type cannot be empty or whitespace.")
                .Must(type => ValidPolicyTypes.Contains(type, StringComparer.OrdinalIgnoreCase))
                    .WithMessage($"Policy type must be one of: {string.Join(", ", ValidPolicyTypes)}.")
                .MaximumLength(50).WithMessage("Policy type cannot exceed 50 characters.");

            // Premium Amount validation
            RuleFor(p => p.PremiumAmount)
                .NotNull().WithMessage("Premium amount is required.")
                .GreaterThan(0).WithMessage("Premium amount must be greater than zero.")
                .LessThanOrEqualTo(999999999999.99m).WithMessage("Premium amount is unrealistically high.")
                .PrecisionScale(18, 2, false).WithMessage("Premium amount must have a maximum of 2 decimal places and fit within DECIMAL(18,2).");

            // Start Date validation
            RuleFor(p => p.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .Must(date => date != default(DateTime)).WithMessage("Start date cannot be default (01/01/0001).");

            // End Date validation
            RuleFor(p => p.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .Must(date => date != default(DateTime)).WithMessage("End date cannot be default (01/01/0001).")
                .GreaterThan(p => p.StartDate)
                    .WithMessage("End date must be after start date.")
                .Must((dto, endDate) => endDate > dto.StartDate.AddDays(1))
                    .WithMessage("Policy must be valid for at least 1 day.")
                .LessThanOrEqualTo(p => p.StartDate.AddYears(10))
                    .WithMessage("Policy duration cannot exceed 10 years.");

            // IsActive validation with business logic
            RuleFor(p => p.IsActive)
                .NotNull().WithMessage("IsActive status is required.");

            // Cross-field validation: Date range check
            RuleFor(p => p)
                .Must(dto => dto.StartDate < dto.EndDate)
                    .WithMessage("Start date must be before end date.")
                .Must(dto => dto.StartDate != dto.EndDate)
                    .WithMessage("Start date and end date cannot be the same.");

            // Business rule: Cannot activate policy outside valid date range
            When(p => p.IsActive, () =>
            {
                RuleFor(p => p.StartDate)
                    .LessThanOrEqualTo(DateTime.Today.AddDays(90))
                        .WithMessage("Cannot activate a policy with start date more than 90 days in the future.");

                RuleFor(p => p.EndDate)
                    .GreaterThanOrEqualTo(DateTime.Today)
                        .WithMessage("Cannot activate a policy that has already ended.");
            });
        }
    }
}
