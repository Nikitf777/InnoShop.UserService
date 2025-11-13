using InnoShop.UserService.Exceptions;
using InnoShop.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.UserService.Repositories;

public interface IUserRepository
{
	public Task<User> FetchByEmailAsync(string email);
	public Task InsertUserAsync(User user);
}

public class UserRepository(UserContext context) : IUserRepository
{
	private readonly UserContext context = context;
	public async Task<User> FetchByEmailAsync(string email)
	{
		try {
			return await this.context.Users.Where(user => user.Email == email).FirstAsync();
		} catch (InvalidOperationException) {
			throw new NotFoundException($"A user with email {email} was not found.");
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
}