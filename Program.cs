using System.Text;
using InnoShop.CommonEnvironment;
using InnoShop.UserService;
using InnoShop.UserService.Exceptions;
using InnoShop.UserService.Models;
using InnoShop.UserService.Repositories;
using InnoShop.UserService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using (var context = new UserContext()) {
	if (!context.Users.Any()) {
		_ = context.Users.Add(new User {
			Name = "admin",
			Email = "admin@innoshop.com",
			PasswordHash = PasswordHasher.HashPassword("admin"),
			Role = Roles.Admin,
			EmailConfirmed = true,
		});

		_ = context.SaveChanges();
	}
}

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
	.AddAuthorization()
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters {
		IssuerSigningKey = new SymmetricSecurityKey(Env.JwtSecretKeyBytes),
		ValidIssuer = Env.JwtKeyIssuer,
		ValidAudience = Env.JwtKeyAudience,
		ValidateIssuerSigningKey = true,
		ValidateLifetime = true,
		ValidateIssuer = true,
		ValidateAudience = true,

	});

builder.Services.Configure<EmailConfiguration>(
	builder.Configuration.GetSection(nameof(EmailConfiguration))
);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>();
builder.Services
	.AddTransient<IUserRepository, UserRepository>()
	.AddTransient<IAuthService, AuthService>()
	.AddTransient<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();
app.Run();
