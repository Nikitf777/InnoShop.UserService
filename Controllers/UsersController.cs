using InnoShop.UserService.Models;
using InnoShop.UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class UsersController(IUserService adminService) : ControllerBase
{
	private readonly IUserService userService = adminService;
	[HttpGet("users")]
	public async Task<IEnumerable<UserDto>> GetUsers()
	{
		return await this.userService.GetUsers();
	}

	[HttpGet("{id}")]
	public async Task<User> GetSpecificUser(uint id)
	{
		return await this.userService.GetSpecificUser(id);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeactivateUser(uint id)
	{
		await this.userService.DeactivateUser(id);
		return this.Ok();
	}

	[HttpPut("activate/{id}")]
	public async Task<IActionResult> ActivateUser(uint id)
	{
		await this.userService.ActivateUser(id);
		return this.Ok();
	}
}