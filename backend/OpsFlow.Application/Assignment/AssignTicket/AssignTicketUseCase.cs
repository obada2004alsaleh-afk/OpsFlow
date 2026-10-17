
using OpsFlow.Domain.Entities;
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Domain.Constants;

namespace OpsFlow.Application.Assignment.AssignTicket
{
    public class AssignTicketUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITechnicianProfileRepository _technicianProfileRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignTicketUseCase(IUserRepository userRepository, ITicketRepository ticketRepository, ITechnicianProfileRepository technicianProfileRepository, IAssignmentRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _technicianProfileRepository = technicianProfileRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<AssignTicketResponse> ExecuteAsync(int currentUserId, AssignTicketRequest request)
        {
            var user = await _userRepository.GetByUserIdAsync(currentUserId);

            if (user == null)
            {
                throw new KeyNotFoundException("not found user");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("user ot active");
            }

            if (user.Role != UserRoles.Admin && user.Role != UserRoles.Manager)
            {
                throw new UnauthorizedAccessException("not allow");
            }

            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException("not found ticket");
            }

            if (ticket.Status == TicketStatuses.Resolved || ticket.Status == TicketStatuses.Cancelled || ticket.Status == TicketStatuses.Closed)
            {
                throw new InvalidOperationException(
                "Ticket cannot be assigned in its current status.");
            }


            var technical = await _technicianProfileRepository.GetByIdAsync(request.TechnicalId);

            if (technical == null)
            {
                throw new KeyNotFoundException("not found technical");
            }
            var technicianUser = technical.User
              ?? throw new InvalidOperationException(
                 "Technician user information is missing.");

            if (!technical.IsAvailable)
            {
                throw new InvalidOperationException("Technical not available");
            }

            if (technicianUser.Role != UserRoles.Technician)
            {
                throw new InvalidOperationException(" not allow");
            }

            if (technicianUser.CustomerId != null)
            {
                throw new InvalidOperationException(" not allow");
            }
            if (!technicianUser.IsActive)
            {
                throw new InvalidOperationException(" not allow");
            }

            var activeAssignment = await _assignmentRepository.GetActiveByTicketIdAsync(request.TicketId);

            if (activeAssignment != null)
            {
                if (activeAssignment.TechnicalId == request.TechnicalId)
                {
                    throw new InvalidOperationException(
                        "Ticket is already assigned to this technician.");
                }
                activeAssignment.IsActive = false;
            }

            var assignment = new OpsFlow.Domain.Entities.Assignment
            {
                TicketId = request.TicketId,
                IsActive = true,
                AssignedAt = DateTime.UtcNow,
                TechnicalId = request.TechnicalId,
                AssignedByUserId = user.UserId


            };


            await _assignmentRepository.SaveAssignmentAsync(activeAssignment, assignment);

            return new AssignTicketResponse
            {
                AssignmentId = assignment.AssignmentId,
                TicketId = assignment.TicketId,
                TechnicalId = assignment.TechnicalId,
                IsActive = assignment.IsActive,
                AssignedAt = assignment.AssignedAt,
                AssignedByUserId = assignment.AssignedByUserId


            };
        }
    }
}
