using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Authentication.Login;
using OpsFlow.Infrastructure.Database;
using OpsFlow.Infrastructure.Repositories;
using OpsFlow.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<LoginUseCase>();

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