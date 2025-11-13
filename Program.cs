using System.Text;
using InnoShop.UserService;
using InnoShop.UserService.Exceptions;
using InnoShop.UserService.Repositories;
using InnoShop.UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>();
builder.Services
	.AddTransient<IUserRepository, UserRepository>()
	.AddTransient<IAuthService, AuthService>()
	.AddTransient<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

app.MapControllers();
app.UseExceptionHandler();
app.Run();
