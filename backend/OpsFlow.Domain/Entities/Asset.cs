
namespace OpsFlow.Domain.Entities
{
    public class Asset
    {
        public int AssetId { get; set; }

        public required string Name { get; set; }

        public required int SiteId { get; set; }
        public Site? Site { get; set; } //navigation property to Site entity

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
