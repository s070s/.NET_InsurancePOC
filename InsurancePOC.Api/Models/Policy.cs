namespace InsurancePOC.Api.Models
{
    /// <summary>
    /// Represents an insurance policy issued to a client.
    /// Tracks policy details, coverage period, and active status.
    /// </summary>
    public class Policy
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; } = "";
        public string PolicyType { get; set; } = "";
        public decimal PremiumAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        
        // Audit field: automatically set when record is created
        public DateTime CreatedAt { get; set; }

        // Foreign key and navigation property for the owning client
        public int ClientId { get; set; }
        public Client? Client { get; set; }
    }
}