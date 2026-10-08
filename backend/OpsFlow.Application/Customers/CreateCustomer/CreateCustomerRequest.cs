
namespace OpsFlow.Application.Customers.CreateCustomer
{
    public class CreateCustomerRequest
    {

        public required string Name { get; set; }


        public required string Email { get; set; }

        public string? Phone { get; set; }

    }
}
