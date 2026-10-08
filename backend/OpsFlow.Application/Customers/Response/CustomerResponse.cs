namespace OpsFlow.Application.Customers.Response
{
    public class CustomerResponse
    {
        public int CustomerId { get; set; }

        public required string Name { get; set; }


        public required string Email { get; set; }

        public string? Phone { get; set; }


        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
