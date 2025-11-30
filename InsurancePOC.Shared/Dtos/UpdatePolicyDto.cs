namespace InsurancePOC.Shared.Dtos;

/// <summary>
/// Data transfer object for updating an existing policy.
/// Contains all mutable fields (excludes Id, ClientId, and CreatedAt).
/// </summary>
public class UpdatePolicyDto
{
    public string PolicyNumber { get; set; } = "";
    public string PolicyType { get; set; } = "";
    public decimal PremiumAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
