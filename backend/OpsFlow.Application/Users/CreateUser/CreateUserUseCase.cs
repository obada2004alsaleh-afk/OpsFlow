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
        private readonly ICustomerRepository _customerRepository;
        public CreateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, ICustomerRepository customerRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _customerRepository = customerRepository;
        }

        public async Task<UserResponse> ExecuteAsync(CreateUserRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                     "A user with this email already exists.");
            }

            if (request.Role != UserRoles.Admin && request.Role != UserRoles.Manager && request.Role != UserRoles.Technician && request.Role != UserRoles.Customer)
            {
                throw new ArgumentException("Invalid role. Role must be Admin, Manager, Technician, or Customer.");
            }
            if (request.Role == UserRoles.Customer)
            {

                if (!request.CustomerId.HasValue)
                {
                    throw new ArgumentException(
                        "CustomerId is required for Customer users.");
                }
                if (request.CustomerId.Value <= 0)
                {
                    throw new ArgumentException("Invalid CustomerId. CustomerId must be a positive integer.");
                }
                var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId.Value);
                if (customer == null)
                {
                    throw new KeyNotFoundException(
                        $"Customer with ID {request.CustomerId.Value} not found.");
                }


                if (!customer.IsActive)
                {
                    throw new InvalidOperationException("Cannot create a user for an inactive customer.");
                }


            }
            else
            {
                if (request.CustomerId.HasValue)
                {
                    throw new ArgumentException(
                        "Internal users cannot have a CustomerId.");
                }
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
                CustomerId = request.Role == UserRoles.Customer
               ? request.CustomerId
               : null
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
                CreatedAt = newUser.CreatedAt,
                CustomerId = newUser.CustomerId
            };
        }
    }
}
