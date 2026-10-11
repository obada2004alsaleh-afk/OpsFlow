
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Application.Tickets.Request_response;
using OpsFlow.Domain.Constants;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Tickets.CreateTicket
{
    public class CreateTicketUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public CreateTicketUseCase(ITicketRepository ticketRepository, ISiteRepository siteRepository, IAssetRepository assetRepository, IUserRepository userRepository, ICustomerRepository customerRepository)
        {
            _ticketRepository = ticketRepository;
            _siteRepository = siteRepository;
            _assetRepository = assetRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }

        public async Task<TicketResponse> ExecuteAsync(CreateTicketRequest createTicketRequest, int currentUserId)
        {

            var allowedCategories = new[]
 {
    TicketCategories.Electrical,
    TicketCategories.Electronics,
    TicketCategories.Mechanical,
    TicketCategories.IT,
    TicketCategories.HVAC
};


            if (currentUserId <= 0)
            {
                throw new ArgumentException("Invalid UserId.");
            }

            if (createTicketRequest.SiteId <= 0)
            {
                throw new ArgumentException("SiteId must be greater than zero.");
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

            if (user.Role != UserRoles.Admin && user.Role != UserRoles.Customer && user.Role != UserRoles.Manager)
            {
                throw new UnauthorizedAccessException(
                   "User is not allowed to create tickets.");
            }
            if (string.IsNullOrWhiteSpace(createTicketRequest.Title))
            {
                throw new ArgumentException("Title is required.");
            }

            if (createTicketRequest.Title.Trim().Length > 200)
            {
                throw new ArgumentException(
                    "Title cannot exceed 200 characters.");
            }
            if (string.IsNullOrWhiteSpace(createTicketRequest.Description))
            {
                throw new ArgumentException("Description is required.");
            }

            if (createTicketRequest.Description.Trim().Length > 1000)
            {
                throw new ArgumentException(
                    "Description cannot exceed 1000 characters.");
            }

            if (string.IsNullOrWhiteSpace(createTicketRequest.Category))
            {
                throw new ArgumentException("Category is required.");
            }
            if (!allowedCategories.Contains(createTicketRequest.Category.Trim()))
            {
                throw new ArgumentException(
                    "Invalid category. Please select a valid ticket category.");
            }

            var priority = TicketPriorities.Medium;

            if (createTicketRequest.Priority != null)
            {
                if (user.Role == UserRoles.Customer)
                {
                    throw new UnauthorizedAccessException(
                        "Customer cannot set ticket priority.");
                }

                var requestedPriority = createTicketRequest.Priority.Trim();

                var allowedPriorities = new[]
                {
        TicketPriorities.Low,
        TicketPriorities.Medium,
        TicketPriorities.High,
        TicketPriorities.Critical
    };

                if (!allowedPriorities.Contains(requestedPriority))
                {
                    throw new ArgumentException("Invalid ticket priority.");
                }

                priority = requestedPriority;
            }
            var site = await _siteRepository.GetByIdAsync(createTicketRequest.SiteId);

            if (site == null)
            {
                throw new KeyNotFoundException("site is null");
            }
            if (user.Role == UserRoles.Customer)
            {
                if (user.CustomerId == null ||
                    user.CustomerId != site.CustomerId)
                {
                    throw new UnauthorizedAccessException(
                        "You cannot create a ticket for this site.");
                }
            }
            if (!site.IsActive)
            {
                throw new InvalidOperationException($"site with ID {createTicketRequest.SiteId} is not active.");
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(site.CustomerId);
            if (customer == null)
            {
                throw new KeyNotFoundException("customer is null");
            }
            if (!customer.IsActive)
            {
                throw new InvalidOperationException("customer is inactive.");
            }

            if (createTicketRequest.AssetId is int assetId)
            {
                if (assetId <= 0)
                {
                    throw new ArgumentException("AssetId must be greater than zero.");
                }

                var asset = await _assetRepository.GetByIdAsync(assetId);

                if (asset == null)
                {
                    throw new KeyNotFoundException("asset is null");
                }

                if (!asset.IsActive)
                {
                    throw new InvalidOperationException($"asset with ID {createTicketRequest.AssetId} is not active.");
                }

                if (asset.SiteId != site.SiteId)
                {
                    throw new InvalidOperationException(
                        "Asset does not belong to the selected Site.");
                }
            }




            var newTicket = new Ticket
            {
                Title = createTicketRequest.Title.Trim(),
                Description = createTicketRequest.Description.Trim(),
                Category = createTicketRequest.Category.Trim(),
                Priority = priority,
                SiteId = createTicketRequest.SiteId,
                AssetId = createTicketRequest.AssetId,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = currentUserId,
            };

            await _ticketRepository.AddAsync(newTicket);
            await _ticketRepository.SaveChangesAsync();


            return new TicketResponse
            {
                TicketId = newTicket.TicketId,
                Title = newTicket.Title,
                Description = newTicket.Description,
                Category = newTicket.Category,
                Priority = newTicket.Priority,
                Status = newTicket.Status,
                SiteId = newTicket.SiteId,
                AssetId = newTicket.AssetId,
                CreatedByUserId = newTicket.CreatedByUserId,
                CreatedAt = newTicket.CreatedAt

            };

        }

    }
}
