
namespace OpsFlow.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public string? Phone { get; set; }

        public required string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public required string Role { get; set; }
 
        public bool IsActive { get; set; }

        public int? CustomerId { get; set; }

    }
}
