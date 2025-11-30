namespace InsurancePOC.Shared.Dtos;

/// <summary>
/// Data transfer object for updating an existing client.
/// Contains all mutable fields (excludes Id and CreatedAt).
/// </summary>
public class UpdateClientDto
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
}