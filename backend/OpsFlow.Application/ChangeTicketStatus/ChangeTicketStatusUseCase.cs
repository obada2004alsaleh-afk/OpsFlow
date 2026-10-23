using AssignmentEntity = OpsFlow.Domain.Entities.Assignment;
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Domain.Constants;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.ChangeTicketStatus
{
    public class ChangeTicketStatusUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public ChangeTicketStatusUseCase(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IAssignmentRepository assignmentRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
        }

        private static readonly Dictionary<string, HashSet<string>>
            AllowedTransitions = new()
            {
                [TicketStatuses.New] = new()
                {
                    TicketStatuses.Cancelled
                },

                [TicketStatuses.Assigned] = new()
                {
                    TicketStatuses.InProgress,
                    TicketStatuses.Cancelled
                },

                [TicketStatuses.InProgress] = new()
                {
                    TicketStatuses.Resolved,
                    TicketStatuses.Cancelled
                },

                [TicketStatuses.Resolved] = new()
                {
                    TicketStatuses.Closed,
                    TicketStatuses.Reopened
                },

                [TicketStatuses.Closed] = new()
                {
                    TicketStatuses.Reopened
                },

                [TicketStatuses.Reopened] = new()
                {
                    TicketStatuses.InProgress,
                    TicketStatuses.Cancelled
                }
            };

        private static readonly Dictionary<string, HashSet<string>>
            AllowedRoles = new()
            {
                [TicketStatuses.InProgress] = new()
                {
                    UserRoles.Admin,
                    UserRoles.Manager,
                    UserRoles.Technician
                },

                [TicketStatuses.Resolved] = new()
                {
                    UserRoles.Admin,
                    UserRoles.Manager,
                    UserRoles.Technician
                },

                [TicketStatuses.Closed] = new()
                {
                    UserRoles.Admin,
                    UserRoles.Manager
                },

                [TicketStatuses.Reopened] = new()
                {
                    UserRoles.Manager
                },

                [TicketStatuses.Cancelled] = new()
                {
                    UserRoles.Admin,
                    UserRoles.Manager
                }
            };


        private void ValidateStatusChange(
            string currentStatus,
            string toStatus,
            string userRole)
        {
            if (string.IsNullOrWhiteSpace(toStatus))
            {
                throw new ArgumentException(
                    "Target status is required.");
            }

            if (currentStatus == toStatus)
            {
                throw new InvalidOperationException(
                    "Ticket already has this status.");
            }

            if (!AllowedTransitions.TryGetValue(
                    currentStatus, out var allowedStatuses)
                || !allowedStatuses.Contains(toStatus))
            {
                throw new InvalidOperationException(
                    "Invalid ticket status transition.");
            }

            if (!AllowedRoles.TryGetValue(
                    toStatus, out var allowedRoles)
                || !allowedRoles.Contains(userRole))
            {
                throw new UnauthorizedAccessException(
                    "User is not allowed to change ticket status.");
            }
        }


        private async Task<AssignmentEntity?>
            ValidateAndGetAssignmentToDeactivateAsync(
                int ticketId,
                int userId,
                string userRole,
                string toStatus)
        {
            bool requiresActiveAssignment =
                toStatus == TicketStatuses.InProgress ||
                toStatus == TicketStatuses.Resolved;

            bool shouldDeactivateAssignment =
                toStatus == TicketStatuses.Closed ||
                toStatus == TicketStatuses.Cancelled;

            AssignmentEntity? activeAssignment = null;

            if (requiresActiveAssignment || shouldDeactivateAssignment)
            {
                activeAssignment =
                    await _assignmentRepository
                        .GetActiveByTicketIdAsync(ticketId);
            }

            if (requiresActiveAssignment)
            {
                if (activeAssignment == null)
                {
                    throw new InvalidOperationException(
                        "Ticket has no active assignment.");
                }

                if (userRole == UserRoles.Technician &&
                    activeAssignment.TechnicianProfile.UserId != userId)
                {
                    throw new UnauthorizedAccessException(
                        "Technician is not assigned to this ticket.");
                }
            }

            return shouldDeactivateAssignment
                ? activeAssignment
                : null;
        }


        private string? ValidateAndNormalizeReason(
            string toStatus,
            string? reason)
        {
            if ((toStatus == TicketStatuses.Reopened ||
                 toStatus == TicketStatuses.Cancelled)
                && string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException(
                    "Reason is required for reopening or cancelling a ticket.");
            }

            string? normalizedReason = reason?.Trim();

            if (normalizedReason?.Length > 500)
            {
                throw new ArgumentException(
                    "Reason cannot exceed 500 characters.");
            }

            return normalizedReason;
        }


        public async Task<ChangeTicketStatusResponse> ExecuteAsync(
            int ticketId,
            int userId,
            ChangeTicketStatusRequest request)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found.");
            }

            var user = await _userRepository.GetByUserIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "User is not active.");
            }

            ValidateStatusChange(
                ticket.Status,
                request.ToStatus,
                user.Role);

            var assignmentToDeactivate =
                await ValidateAndGetAssignmentToDeactivateAsync(
                    ticketId,
                    userId,
                    user.Role,
                    request.ToStatus);


            string? reason = ValidateAndNormalizeReason(
                request.ToStatus,
                request.Reason);


            string fromStatus = ticket.Status;


            var history = new TicketStatusHistory
            {
                TicketId = ticketId,
                FromStatus = fromStatus,
                ToStatus = request.ToStatus,
                ChangedByUserId = userId,
                ChangedAt = DateTime.UtcNow,
                Reason = reason
            };

            await _ticketRepository.SaveStatusChangeAsync(
                ticket,
                history,
                assignmentToDeactivate);

            return new ChangeTicketStatusResponse
            {
                TicketStatusHistoryId = history.TicketStatusHistoryId,
                TicketId = history.TicketId,
                FromStatus = history.FromStatus,
                ToStatus = history.ToStatus,
                ChangedByUserId = history.ChangedByUserId,
                ChangedAt = history.ChangedAt,
                Reason = history.Reason
            };
        }
    }
}