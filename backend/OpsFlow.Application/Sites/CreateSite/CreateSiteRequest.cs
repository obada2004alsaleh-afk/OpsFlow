
namespace OpsFlow.Application.Sites.CreateSite
{
    public class CreateSiteRequest
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public required string Address { get; set; }
        public int CustomerId { get; set; }
    }
}
