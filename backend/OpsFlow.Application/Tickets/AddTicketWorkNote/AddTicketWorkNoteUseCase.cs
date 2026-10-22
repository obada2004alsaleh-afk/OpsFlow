
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Domain.Constants;

namespace OpsFlow.Application.Tickets.AddTicketWorkNote
{
    public class AddTicketWorkNoteUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentRepository _assignmentRepository;


        public AddTicketWorkNoteUseCase(ITicketRepository ticketRepository,IUserRepository userRepository,IAssignmentRepository assignmentRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
        }


        public async Task ExecuteAsync(int ticketId,int userId,string info)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

           if(ticket == null)
            {
                throw new KeyNotFoundException("not found ticket");
            }

            if (ticket.Status != TicketStatuses.InProgress)
            {
                throw new InvalidOperationException(
                    "Work notes can only be added while the ticket is in progress.");
            }

            var user = await _userRepository.GetByUserIdAsync(userId);

            if(user ==null)
            {
                throw new KeyNotFoundException("not found user");
            }
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "User is not active.");
            }

            if (user.Role != UserRoles.Technician)
            {
                throw new UnauthorizedAccessException(
                    "Only technicians can add work notes.");
            }

            var assign = await _assignmentRepository.GetActiveByTicketIdAsync(ticketId);
            if(assign == null)
            {

                throw new InvalidOperationException(
                    "Ticket has no active assignment.");
            }
            if (assign.TechnicianProfile.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You are not assigned to this ticket.");
            }
            if (string.IsNullOrWhiteSpace(info))
            {
                throw new ArgumentException("info is required.");
            }


            string note = info.Trim();

            if (note.Length > 2000)
            {
                throw new ArgumentException(
                    "Work note cannot exceed 2000 characters.");
            }

            string separator =
                string.IsNullOrWhiteSpace(ticket.WorkNotes)
                    ? ""
                    : Environment.NewLine + Environment.NewLine;

            string newEntry =
                $"[UserId: {userId} | UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}]" +
                Environment.NewLine +
                note;

            ticket.WorkNotes =
                (ticket.WorkNotes ?? "") + separator + newEntry;

            await _ticketRepository.SaveChangesAsync();


        }


    }
}
