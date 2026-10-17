

using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Application.Tickets.Request_response;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.TechnicianProfileRepository.AssignTicket
{
    public class TechnicianProfileUseCase
    {
        private readonly ITechnicianProfileRepository _technicianProfileRepository;
        private readonly IUserRepository _userRepository;
        public TechnicianProfileUseCase(ITechnicianProfileRepository technicianProfileRepository,IUserRepository userRepository)
        {
            _technicianProfileRepository = technicianProfileRepository;
            _userRepository = userRepository;
        }

       public async Task<TechinicalProfileResponse> ExecuteAsync(int currentUserId , TechnicianProfileRequest technicianProfileRequest)
        {

             if(technicianProfileRequest.UserId <= 0)
               {
                throw new ArgumentException("UserId must be greater than zero.");

               }
            var currentuser = await _userRepository.GetByUserIdAsync(currentUserId);

            if(currentuser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if(currentuser.Role!= UserRoles.Admin && currentuser.Role != UserRoles.Manager)
            {
                throw new UnauthorizedAccessException(
      "Only Admin or Manager can create technician profiles.");
            }

            if(!currentuser.IsActive)
            {
                throw new UnauthorizedAccessException("not Active");
            }

            var user = await _userRepository.GetByUserIdAsync(technicianProfileRequest.UserId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (user.Role != UserRoles.Technician || !user.IsActive)
            {
                throw new ArgumentException("not alowed");
            }
            if (string.IsNullOrWhiteSpace(technicianProfileRequest.Specialization))
            {
                throw new ArgumentException("Specialization is required.");
            }
            if (user.TechnicalProfile != null)
            {

                    throw new InvalidOperationException(
                        "User already has a technician profile.");

            }

            if (user.CustomerId != null)
            {
                throw new InvalidOperationException(
                    "Technician cannot belong to a customer.");
            }

            var newTechnicianProfile = new TechnicianProfile
            {
                Specialization = technicianProfileRequest.Specialization,
                IsAvailable = true,
                UserId = technicianProfileRequest.UserId
            };

            await _technicianProfileRepository.AddAsync(newTechnicianProfile);
            await _technicianProfileRepository.SaveChangesAsync();

            return new TechinicalProfileResponse
            {
                TechnicalId = newTechnicianProfile.TechnicalId,
                Specialization = newTechnicianProfile.Specialization,
                IsAvailable = newTechnicianProfile.IsAvailable,
                UserId = newTechnicianProfile.UserId
            };

        }


    }
}
