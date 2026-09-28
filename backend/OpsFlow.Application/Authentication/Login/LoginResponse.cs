using System;
using System.Collections.Generic;
using System.Text;

namespace OpsFlow.Application.Authentication.Login
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
    }
}
