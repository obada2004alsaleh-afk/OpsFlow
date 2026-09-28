
using System.Security.Claims;
using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace OpsFlow.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _secretKey;
         
           public JwtTokenGenerator(string secretKey)
            {
                _secretKey = secretKey;
            }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("userId",user.UserId.ToString()),
                new Claim("role",user.Role.ToString())
            };

            var keyBytes = Convert.FromBase64String(_secretKey); //convert to byte
            var key = new SymmetricSecurityKey(keyBytes); //convert to object

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                 );

            var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(30),
           signingCredentials: credentials
           );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);//convert to jwt token

        }

    }
}
