

namespace OpsFlow.Application.Assets.Response
{
    public class AssetResponse
    {
        public int AssetId { get; set; }

        public required string Name { get; set; }

        public int SiteId { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

    }
}
