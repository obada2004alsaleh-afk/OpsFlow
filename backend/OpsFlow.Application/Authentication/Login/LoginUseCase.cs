using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Authentication.Login
{
    public class LoginUseCase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public LoginUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse?> ExecuteAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            if (_passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                var token = _jwtTokenGenerator.GenerateToken(user);

                return new LoginResponse
                {
                    UserId = user.UserId,
                    Role = user.Role,
                    Token = token
                };
            }

            return null;
        }


    }
}
