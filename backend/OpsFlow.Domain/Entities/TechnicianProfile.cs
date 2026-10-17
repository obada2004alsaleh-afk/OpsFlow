

namespace OpsFlow.Domain.Entities
{
    public class TechnicianProfile
    {
        public int TechnicalId { get; set; }
        public required string Specialization { get; set; }
        public bool IsAvailable { get; set; }

        public required int UserId { get; set; }
        public   User? User { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = [];
    }
}
