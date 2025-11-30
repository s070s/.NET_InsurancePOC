namespace InsurancePOC.Shared.Dtos;

/// <summary>
/// Data transfer object for creating a new insurance policy.
/// Requires a valid ClientId. IsActive defaults to true, CreatedAt is set by the system.
/// </summary>
public class CreatePolicyDto
{
    public int ClientId { get; set; }
    public string PolicyNumber { get; set; } = "";
    public string PolicyType { get; set; } = "";
    public decimal PremiumAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
