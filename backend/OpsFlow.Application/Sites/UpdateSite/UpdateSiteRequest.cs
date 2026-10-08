

namespace OpsFlow.Application.Sites.UpdateSite
{
    public class UpdateSiteRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public bool? IsActive { get; set; }
    }
}
