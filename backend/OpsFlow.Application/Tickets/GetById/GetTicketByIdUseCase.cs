
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Application.Tickets.Request_response;

namespace OpsFlow.Application.Tickets.GetById
{
    public class GetTicketByIdUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISiteRepository _siteRepository;

        public GetTicketByIdUseCase(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            ISiteRepository siteRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _siteRepository = siteRepository;
        }

        public async Task<TicketResponse> ExecuteAsync(
            int ticketId,
            int currentUserId)
        {
            // 1. Validate IDs
            if (currentUserId <= 0)
            {
                throw new ArgumentException("Invalid UserId.");
            }

            if (ticketId <= 0)
            {
                throw new ArgumentException("Invalid TicketId.");
            }


            var user = await _userRepository
                .GetByUserIdAsync(currentUserId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("User is inactive.");
            }


            if (user.Role != UserRoles.Admin &&
                user.Role != UserRoles.Manager &&
                user.Role != UserRoles.Customer)
            {
                throw new UnauthorizedAccessException(
                    "User is not allowed to view tickets.");
            }


            var ticket = await _ticketRepository
                .GetByIdAsync(ticketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found.");
            }

            if (user.Role == UserRoles.Customer)
            {
                if (!user.CustomerId.HasValue)
                {
                    throw new UnauthorizedAccessException(
                        "Customer user is not linked to a company.");
                }

                var site = await _siteRepository
                    .GetByIdAsync(ticket.SiteId);

                if (site == null ||
                    site.CustomerId != user.CustomerId.Value)
                {
                    throw new UnauthorizedAccessException(
                        "You cannot access this ticket.");
                }
            }

            return new TicketResponse
            {
                TicketId = ticket.TicketId,
                Title = ticket.Title,
                Description = ticket.Description,
                Category = ticket.Category,
                Priority = ticket.Priority,
                Status = ticket.Status,
                SiteId = ticket.SiteId,
                AssetId = ticket.AssetId,
                CreatedByUserId = ticket.CreatedByUserId,
                CreatedAt = ticket.CreatedAt,
                WorkNotes =
                user.Role == UserRoles.Admin ||
                user.Role == UserRoles.Manager
                ? ticket.WorkNotes
                 : null
            };
        }
    }
}
