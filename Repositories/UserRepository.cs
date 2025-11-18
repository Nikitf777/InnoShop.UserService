using InnoShop.UserService.Exceptions;
using InnoShop.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.UserService.Repositories;

public interface IUserRepository
{
	public Task<IEnumerable<UserDto>> FetchAllAsync();
	public Task<IEnumerable<UserDto>> FetchAllActivatedAsync();
	public Task<User> FetchByIdAsync(uint id);
	public Task<User> FetchByEmailAsync(string email);
	public Task InsertUserAsync(User user);
	public Task DeactivateUserAsync(uint id);
	public Task ActivateUserAsync(uint id);
}

public class UserRepository(UserContext context) : IUserRepository
{
	private readonly UserContext context = context;

	public async Task<IEnumerable<UserDto>> FetchAllAsync()
	{
		return await (
			from user in this.context.Users
			select new UserDto(user)
		).ToListAsync();
	}

	public async Task<IEnumerable<UserDto>> FetchAllActivatedAsync()
	{
		return await (
			from user in this.context.Users
			where !user.Deactivated
			select new UserDto(user)
		).ToListAsync();
	}


	public async Task<User> FetchByIdAsync(uint id)
	{
		try {
			return await this.context.Users.Where(user => user.Id == id).FirstAsync();
		} catch (InvalidOperationException) {
			throw new NotFoundException($"A user with {nameof(id)} {id} was not found.");
		}
	}
	public async Task<User> FetchByEmailAsync(string email)
	{
		try {
			return await this.context.Users.Where(user => user.Email == email).FirstAsync();
		} catch (InvalidOperationException) {
			throw new NotFoundException($"A user with {nameof(email)} {email} was not found.");
		}
	}

	public async Task InsertUserAsync(User user)
	{
		_ = this.context.Users.Add(user);
		try {
			_ = await this.context.SaveChangesAsync();

		} catch (DbUpdateException e) {
			if (e.InnerException is null) {
				throw;
			}

			if (e.InnerException.Data["SqlState"] is not string str || str != "23505") {
				throw;
			}

			if (e.InnerException.Data["ConstraintName"] is not string constraintStr) {
				throw;
			}

			if (constraintStr.Contains(nameof(User.Name))) {
				throw new UserExistsException($"A user with the same name already exists");
			} else if (constraintStr.Contains(nameof(User.Email))) {
				throw new UserExistsException($"A user with the same email already exists");
			}
		}
	}

	public async Task DeactivateUserAsync(uint id)
	{
		if (await this.context.Users
			.Where(user => user.Id == id && !user.Deactivated)
			.ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Deactivated, true)) == 0) {
			throw new NotFoundException($"A deactivated user with {nameof(id)} {id} was not found.");
		}
	}

	public async Task ActivateUserAsync(uint id)
	{
		if (await this.context.Users
			.Where(user => user.Id == id && !user.Deactivated)
			.ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Deactivated, false)) == 0) {
			throw new NotFoundException($"A deactivated user with {nameof(id)} {id} was not found.");
		}
	}
}