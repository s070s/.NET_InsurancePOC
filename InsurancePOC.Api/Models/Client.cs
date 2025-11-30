namespace InsurancePOC.Api.Models
{
    /// <summary>
    /// Represents an insurance client in the system.
    /// Each client can have multiple policies associated with them.
    /// </summary>
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } ="";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = " ";
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        // Audit field: automatically set when record is created
        public DateTime CreatedAt { get; set; }

        // Navigation property for related policies (one-to-many relationship)
        // TODO: Add ICollection<Policy> Policies navigation property when needed
    }
}