
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Application.Tickets.Request_response;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Tickets.GetAll
{
    public class GetAllTicketsUseCase
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        public GetAllTicketsUseCase(ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<List<TicketResponse>> ExecuteAsync(int currentUserId)
        {
            if (currentUserId <= 0)
            {
                throw new ArgumentException("Invalid UserId.");
            }

            var user = await _userRepository.GetByUserIdAsync(currentUserId);

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

            List<Ticket> tickets;

            if (user.Role == UserRoles.Customer)
            {
                if (!user.CustomerId.HasValue)
                {
                    throw new UnauthorizedAccessException(
                        "Customer user is not linked to a company.");
                }

                tickets = await _ticketRepository.GetByCustomerIdAsync(
                    user.CustomerId.Value);
            }
            else
            {
                tickets = await _ticketRepository.GetAllAsync();
            }


            return tickets.Select(ticket => new TicketResponse
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
                CreatedAt = ticket.CreatedAt

            }).ToList();


        }
    }
}
