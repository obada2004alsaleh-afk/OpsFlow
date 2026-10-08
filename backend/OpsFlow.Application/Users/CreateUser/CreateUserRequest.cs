using System;
using System.Collections.Generic;
using System.Text;

namespace OpsFlow.Application.Users.CreateUser
{
    public class CreateUserRequest
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public string? Phone { get; set; }
        public int? CustomerId { get; set; }
        public required string Password { get; set; }

        public required string Role { get; set; }
    }
}
