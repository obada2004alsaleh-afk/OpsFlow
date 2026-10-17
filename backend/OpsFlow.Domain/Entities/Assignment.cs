

namespace OpsFlow.Domain.Entities
{
    public class Assignment
    {
        public int AssignmentId { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime AssignedAt { get; set; }

        public int TechnicalId { get; set; }
        public TechnicianProfile TechnicianProfile { get; set; } = null!;

        public int AssignedByUserId { get; set; }
        public User AssignedByUser { get; set; } = null!;

    }
}
