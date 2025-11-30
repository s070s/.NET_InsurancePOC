namespace InsurancePOC.Shared.Dtos;

/// <summary>
/// Data transfer object for returning client information from the API.
/// </summary>
public class ClientDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
}