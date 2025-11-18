using InnoShop.UserService.Repositories;
using InnoShop.UserService.Models;

namespace InnoShop.UserService.Services;

public interface IAuthService
{
	public Task RegisterUserAsync(string name, string email, string password);
	public Task<(bool isValid, string role)> ValidateUserAsync(string email, string password);
}

public class AuthService(IUserRepository userRepository) : IAuthService
{
	private readonly IUserRepository userRepository = userRepository;

	public async Task RegisterUserAsync(string name, string email, string password)
	{
		await this.userRepository.InsertUserAsync(new User { Name = name, Email = email, PasswordHash = PasswordHasher.HashPassword(password) });
	}

	public async Task<(bool isValid, string role)> ValidateUserAsync(string email, string password)
	{
		var user = await this.userRepository.FetchByEmailAsync(email);
		if (user == null) {
			return (false, string.Empty);
		}

		var isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);

		if (!isValid) {
			return (false, string.Empty);
		}

		return (true, user.Role);
	}
}