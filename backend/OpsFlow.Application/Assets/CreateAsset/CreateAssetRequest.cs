

namespace OpsFlow.Application.Assets.CreateAsset
{
    public class CreateAssetRequest
    {

        public required string Name { get; set; }
        public int SiteId { get; set; }

    }
}
