using InnoShop.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.UserService;

public class UserContext : DbContext
{
	public DbSet<User> Users { get; set; }

	public UserContext()
	{
		_ = this.Database.EnsureCreated();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		_ = optionsBuilder.UseNpgsql($"Host={Env.DbHost};Port={Env.DbPort};Database={Env.DbName};Username={Env.DbUser};Password={Env.DbPassword}");
	}
}