using OpsFlow.Domain.Constants;

namespace OpsFlow.Domain.Entities
{
    public class Ticket
    {

        public int TicketId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = TicketPriorities.Medium;
        public string Status { get; set; } = TicketStatuses.New;

        public int SiteId { get; set; }

        public  Site Site { get; set; } = null!;

        public int? AssetId { get; set; }

        public Asset? Asset { get; set; }


        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
