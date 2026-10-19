

namespace OpsFlow.Application.ChangeTicketStatus
{
    public class ChangeTicketStatusRequest
    {
        public required string ToStatus { get; set; }

        public string? Reason { get; set; }
    }
}
