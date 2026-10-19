

namespace OpsFlow.Application.ChangeTicketStatus
{
    public class ChangeTicketStatusResponse
    {
        public int TicketStatusHistoryId { get; set; }
        public int TicketId { get; set; }
        public required string FromStatus { get; set; }
        public required string ToStatus { get; set; }
        public int ChangedByUserId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Reason { get; set; }
    }
}
