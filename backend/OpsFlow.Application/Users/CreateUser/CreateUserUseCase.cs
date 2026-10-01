using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Application.Role;
using OpsFlow.Application.Users.SharedResponse;

namespace OpsFlow.Application.Users.CreateUser
{
    public class CreateUserUseCase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        public CreateUserUseCase(IUserRepository userRepository,IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse> ExecuteAsync(CreateUserRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
            throw new  InvalidOperationException(
                 "A user with this email already exists.");
            }
            if(request.Role != UserRoles.Admin && request.Role != UserRoles.Manager && request.Role != UserRoles.Technician)
            {
                throw new ArgumentException("Invalid role. Role must be Admin, Manager, or Technician.");
            }

             var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                Role = request.Role,
                IsActive = true,
                CustomerId = null
            };

           await _userRepository.AddAsync(newUser);
           await _userRepository.SaveChangesAsync();

            return new UserResponse
            {
                UserId = newUser.UserId,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Phone = newUser.Phone,
                Role = newUser.Role,
                IsActive = newUser.IsActive,
                CreatedAt = newUser.CreatedAt
            };
        }
    }
}
