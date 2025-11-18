using InnoShop.UserService.Models;
using InnoShop.UserService.Repositories;

namespace InnoShop.UserService.Services;

public interface IUserService
{
	public Task<IEnumerable<UserDto>> GetUsers();
	public Task<User> GetSpecificUser(uint id);
	public Task DeactivateUser(uint id);
	public Task ActivateUser(uint id);
}

public class UserService(IUserRepository userRepository) : IUserService
{
	private readonly IUserRepository userRepository = userRepository;

	public async Task<IEnumerable<UserDto>> GetUsers()
	{
		return await this.userRepository.FetchAllAsync();
	}

	public async Task<User> GetSpecificUser(uint id)
	{
		return await this.userRepository.FetchByIdAsync(id);
	}

	public async Task DeactivateUser(uint id)
	{
		await this.userRepository.DeactivateUserAsync(id);
	}

	public async Task ActivateUser(uint id)
	{
		await this.userRepository.ActivateUserAsync(id);
	}
}