


namespace OpsFlow.Application.Tickets.Request_response
{
    public class CreateTicketRequest
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Priority { get; set; } 

        public int SiteId { get; set; }

        public int? AssetId { get; set; }



    }
}
