
namespace OpsFlow.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public required string Name { get; set; }


        public required string Email { get; set; }

        public string? Phone { get; set; }


        public DateTime CreatedAt { get; set; }

        public ICollection<Site> Sites { get; set; } = []; //navigation property to Site entity
        public ICollection<User> Users { get; set; } = []; //navigation property to User entity

        public bool IsActive { get; set; }

    }
}
