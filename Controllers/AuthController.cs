using System.ComponentModel.DataAnnotations;
using InnoShop.UserService.Exceptions;
using InnoShop.UserService.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IJwtTokenService tokenService) : ControllerBase
{
	private readonly IAuthService authService = authService;
	private readonly IJwtTokenService tokenService = tokenService;

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request)
	{
		await this.authService.RegisterUserAsync(request.Name, request.Email, request.Password);
		return this.Ok();
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		var user = await this.authService.ValidateUserAsync(request.Email, request.Password);
		var token = this.tokenService.GenerateToken(user.Id, request.Email, user.Role);
		return this.Ok(new { Token = token });
	}
}

public class RegisterRequest
{
	[StringLength(40, ErrorMessage = ErrorMessages.NameTooLong)]
	public string Name { get; set; } = "";
	[StringLength(80, ErrorMessage = ErrorMessages.EmailTooLong)]
	public string Email { get; set; } = "";
	[StringLength(80, ErrorMessage = ErrorMessages.PasswordTooLong)]
	public string Password { get; set; } = "";
}

public class LoginRequest
{
	[StringLength(80, ErrorMessage = ErrorMessages.EmailTooLong)]
	public string Email { get; set; } = "";
	[StringLength(80, ErrorMessage = ErrorMessages.PasswordTooLong)]
	public string Password { get; set; } = "";
}
