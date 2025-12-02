using FluentValidation;
using InsurancePOC.Shared.Dtos;

namespace InsurancePOC.Shared.Validation
{
    /// <summary>
    /// FluentValidation validator for CreatePolicyDto.
    /// Ensures policy data meets business rules before being saved to the database.
    /// </summary>
    public class CreatePolicyDtoValidator : AbstractValidator<CreatePolicyDto>
    {
        public CreatePolicyDtoValidator()
        {
            RuleFor(p => p.ClientId).GreaterThan(0).WithMessage("ClientId must be valid");
            RuleFor(p => p.PolicyNumber).NotEmpty().MaximumLength(50);
            RuleFor(p => p.PolicyType).NotEmpty().MaximumLength(100);
            RuleFor(p => p.PremiumAmount).GreaterThan(0).WithMessage("Premium amount must be greater than zero");
            RuleFor(p => p.StartDate).NotEmpty();
            RuleFor(p => p.EndDate).NotEmpty().GreaterThan(p => p.StartDate)
                .WithMessage("End date must be after start date");
        }
    }
}
