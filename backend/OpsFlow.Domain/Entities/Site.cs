

namespace OpsFlow.Domain.Entities
{
    public class Site
    {
        public int SiteId { get; set; }

        public required string Name { get; set; }


        public string? Email { get; set; }

        public string? Phone { get; set; }

        public required string Address { get; set; }


        public DateTime CreatedAt { get; set; }

        public ICollection<Asset> Assets { get; set; } = []; //navigation property to Asset entity

        public bool IsActive { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; } //navigation property to Customer entity

    }
}
