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

            if (string.IsNullOrWhiteSpace(request.ToStatus))
            {
                throw new ArgumentException("Target status is required.");
            }

            if (ticket.Status == request.ToStatus)
            {
                throw new InvalidOperationException(
                    "Ticket already has this status.");
            }

            if (!AllowedTransitions.TryGetValue(
                    ticket.Status, out var allowedStatuses)
                || !allowedStatuses.Contains(request.ToStatus))
            {
                throw new InvalidOperationException(
                    "Invalid ticket status transition.");
            }

            if (!AllowedRoles.TryGetValue(
                    request.ToStatus, out var allowedRoles)
                || !allowedRoles.Contains(user.Role))
            {
                throw new UnauthorizedAccessException(
                    "User is not allowed to change ticket status.");
            }

            bool requiresActiveAssignment =
                request.ToStatus == TicketStatuses.InProgress ||
                request.ToStatus == TicketStatuses.Resolved;

            bool shouldDeactivateAssignment =
                request.ToStatus == TicketStatuses.Closed ||
                request.ToStatus == TicketStatuses.Cancelled;

            AssignmentEntity? activeAssignment = null;

            if (requiresActiveAssignment || shouldDeactivateAssignment)
            {
                activeAssignment =
                    await _assignmentRepository.GetActiveByTicketIdAsync(ticketId);
            }

            if (requiresActiveAssignment)
            {
                if (activeAssignment == null)
                {
                    throw new InvalidOperationException(
                        "Ticket has no active assignment.");
                }

                if (user.Role == UserRoles.Technician &&
                    activeAssignment.TechnicianProfile.UserId != userId)
                {
                    throw new UnauthorizedAccessException(
                        "Technician is not assigned to this ticket.");
                }
            }

            if ((request.ToStatus == TicketStatuses.Reopened ||
                 request.ToStatus == TicketStatuses.Cancelled)
                && string.IsNullOrWhiteSpace(request.Reason))
            {
                throw new ArgumentException(
                    "Reason is required for reopening or cancelling a ticket.");
            }

            string? reason = request.Reason?.Trim();

            if (reason?.Length > 500)
            {
                throw new ArgumentException(
                    "Reason cannot exceed 500 characters.");
            }

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
            AssignmentEntity? assignmentToDeactivate =
    shouldDeactivateAssignment ? activeAssignment : null;

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