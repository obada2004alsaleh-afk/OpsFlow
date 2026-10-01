using OpsFlow.Application.Users.SharedResponse;
using OpsFlow.Application.Interfaces;

namespace OpsFlow.Application.Users.GetUsers
{
    public class GetAllUsersUseCase
    {

        private readonly IUserRepository _userRepository;

        public GetAllUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> ExecuteAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserResponse
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt


            }).ToList();
        }


    }
}
