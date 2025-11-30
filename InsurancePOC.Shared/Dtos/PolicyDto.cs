namespace InsurancePOC.Shared.Dtos;

/// <summary>
/// Data transfer object for returning policy information from the API.
/// </summary>
public class PolicyDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string PolicyNumber { get; set; } = "";
    public string PolicyType { get; set; } = "";
    public decimal PremiumAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
