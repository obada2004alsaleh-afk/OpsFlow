


namespace OpsFlow.Application.Tickets.Request_response
{
    public class TicketResponse
    {
        public int TicketId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int SiteId { get; set; }


        public int? AssetId { get; set; }



        public int CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } 


    

    }
}
