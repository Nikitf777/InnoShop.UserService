using InnoShop.UserService.Repositories;
using InnoShop.UserService.Models;
using InnoShop.UserService.Exceptions;

namespace InnoShop.UserService.Services;

public interface IAuthService
{
	public Task RegisterUserAsync(string name, string email, string password);
	public Task<User> ValidateUserAsync(string email, string password);
}

public class AuthService(IUserRepository userRepository) : IAuthService
{
	private readonly IUserRepository userRepository = userRepository;

	public async Task RegisterUserAsync(string name, string email, string password)
	{
		await this.userRepository.InsertUserAsync(new User { Name = name, Email = email, PasswordHash = PasswordHasher.HashPassword(password) });
	}

	public async Task<User> ValidateUserAsync(string email, string password)
	{
		var user = await this.userRepository.FetchByEmailAsync(email) ?? throw new WrongCredentialsException("Wrong email or password.");

		if (!PasswordHasher.VerifyPassword(password, user.PasswordHash)) {
			throw new WrongCredentialsException("Wrong email or password.");
		}

		return user;
	}
}