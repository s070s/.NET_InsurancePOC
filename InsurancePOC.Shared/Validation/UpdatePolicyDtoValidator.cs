using FluentValidation;
using InsurancePOC.Shared.Dtos;

namespace InsurancePOC.Shared.Validation
{
    /// <summary>
    /// FluentValidation validator for UpdatePolicyDto.
    /// Ensures policy update data meets business rules before being saved to the database.
    /// </summary>
    public class UpdatePolicyDtoValidator : AbstractValidator<UpdatePolicyDto>
    {
        public UpdatePolicyDtoValidator()
        {
            RuleFor(p => p.PolicyNumber).NotEmpty().MaximumLength(50);
            RuleFor(p => p.PolicyType).NotEmpty().MaximumLength(100);
            RuleFor(p => p.PremiumAmount).GreaterThan(0).WithMessage("Premium amount must be greater than zero");
            RuleFor(p => p.StartDate).NotEmpty();
            RuleFor(p => p.EndDate).NotEmpty().GreaterThan(p => p.StartDate)
                .WithMessage("End date must be after start date");
        }
    }
}
