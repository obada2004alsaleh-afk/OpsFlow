
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

        public async Task<UserResponse> ExecuteAsync(
            int userId,
            UpdateUserRequest request)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Validate email uniqueness
            if (request.Email != null && request.Email != user.Email)
            {
                var existingUser =
                    await _userRepository.GetByEmailAsync(request.Email);

                if (existingUser != null)
                {
                    throw new InvalidOperationException(
                        "Email already exists");
                }
            }

            // Validate role and protect Customer relationship
            if (request.Role != null)
            {
                if (request.Role != UserRoles.Admin &&
                    request.Role != UserRoles.Manager &&
                    request.Role != UserRoles.Technician &&
                    request.Role != UserRoles.Customer)
                {
                    throw new ArgumentException("Invalid role.");
                }

                bool wasCustomer =
                    user.Role == UserRoles.Customer;

                bool willBeCustomer =
                    request.Role == UserRoles.Customer;

                if (wasCustomer != willBeCustomer)
                {
                    throw new InvalidOperationException(
                        "Cannot change between Customer and internal roles.");
                }
            }

            // Apply updates after validation
            if (request.Email != null)
            {
                user.Email = request.Email;
            }

            if (request.Role != null)
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

            if (request.IsActive.HasValue)
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
                CreatedAt = user.CreatedAt,
                CustomerId = user.CustomerId
            };
        }
    }
}
