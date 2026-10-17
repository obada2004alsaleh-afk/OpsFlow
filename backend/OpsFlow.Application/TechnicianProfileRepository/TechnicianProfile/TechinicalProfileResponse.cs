

namespace OpsFlow.Application.TechnicianProfileRepository.AssignTicket
{
    public class TechinicalProfileResponse
    {
        public int TechnicalId { get; set; }
        public required string Specialization { get; set; }
        public bool IsAvailable { get; set; }
        public required int UserId { get; set; }


    }
}
