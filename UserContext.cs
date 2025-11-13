using InnoShop.CommonEnvironment;
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

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_ = modelBuilder.Entity<User>()
			.HasIndex(user => user.Name)
			.IsUnique();

		_ = modelBuilder.Entity<User>()
			.HasIndex(user => user.Email)
			.IsUnique();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = $"Host={Env.DbHost};Port={Env.DbPort};Database={Env.DbName};Username={Env.DbUser};Password={Env.DbPassword}";
		_ = optionsBuilder.UseNpgsql(connectionString);
	}
}