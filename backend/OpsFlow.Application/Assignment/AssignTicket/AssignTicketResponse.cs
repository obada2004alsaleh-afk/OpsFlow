



namespace OpsFlow.Application.Assignment.AssignTicket
{
    public class AssignTicketResponse
    {
        public int AssignmentId { get; set; }

        public int TicketId { get; set; }

        public bool IsActive { get; set; }

        public DateTime AssignedAt { get; set; }

        public int TechnicalId { get; set; }

        public int AssignedByUserId { get; set; }

    }
}
