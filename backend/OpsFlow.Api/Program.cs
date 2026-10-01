using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpsFlow.Application.Authentication.Login;
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Users.CreateUser;
using OpsFlow.Infrastructure.Database;
using OpsFlow.Infrastructure.Repositories;
using OpsFlow.Infrastructure.Security;
using OpsFlow.Application.Users.GetUsers;
using OpsFlow.Application.Users.UpdateUser;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();


builder.Services.AddControllers();

var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];

if (string.IsNullOrWhiteSpace(jwtSecretKey))
{
    throw new InvalidOperationException("JWT Secret Key is missing.");
}
// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(jwtSecretKey)
        )
        };
    });


var connectionString=builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<OpsFlowDbContext>(
  option => option.UseSqlServer(connectionString)
    );




builder.Services.AddScoped<IJwtTokenGenerator>(_ =>
    new JwtTokenGenerator(jwtSecretKey));


builder.Services.AddAuthorization();

var app = builder.Build();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//model binding loginRequest from the request body and injecting LoginUseCase from DI container
app.MapPost("/api/auth/login",
    async (LoginRequest loginRequest, LoginUseCase loginUseCase) =>
    {
        var loginResponse = await loginUseCase.ExecuteAsync(loginRequest);

        if (loginResponse == null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(loginResponse);
    });



app.Run();