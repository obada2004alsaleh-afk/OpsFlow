using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Role;
using OpsFlow.Application.Users.SharedResponse;

namespace OpsFlow.Application.Users.UpdateUser
{

    public class UpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> ExecuteAsync(int UserId, UpdateUserRequest request)
        {

            var user = await _userRepository.GetByUserIdAsync(UserId);


            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }




            if (request.Email != null && request.Email != user.Email)
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Email already exists");
                }

            }

            if (request.Role != null)
            {
                if (request.Role != UserRoles.Admin && request.Role != UserRoles.Technician && request.Role != UserRoles.Manager)
                {
                    throw new ArgumentException(
                 "Role must be Admin, Manager, or Technician.");
                }

            }
            if(request.Email != null)
            {
                user.Email = request.Email;
            }
            if (request.Role !=null)
            {
                user.Role = request.Role;
            }

            if (request.FirstName != null)
            {
                user.FirstName = request.FirstName;
            }

            if (request.LastName != null)
            {
                user.LastName = request.LastName;
            }
            if (request.Phone != null)
            {
                user.Phone = request.Phone;
            }


            if (request.IsActive != null)
            {
                user.IsActive = request.IsActive.Value;
            }




            await _userRepository.SaveChangesAsync();

            return new UserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt


            };

        }

    }
}