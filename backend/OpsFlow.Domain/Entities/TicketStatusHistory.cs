

namespace OpsFlow.Domain.Entities
{
    public class TicketStatusHistory
    {
        public int TicketStatusHistoryId { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
        public required string FromStatus { get; set; }
        public required string ToStatus { get; set; }
        public int ChangedByUserId { get; set; }
        public User ChangedByUser { get; set; } = null!;
        public DateTime ChangedAt { get; set; }
        public string? Reason { get; set; }


    }
}
