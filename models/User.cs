namespace InnoShop.UserService.Models;

public static class Roles
{
	public const string Admin = nameof(Admin);
	public const string User = nameof(User);
}

public class User
{
	public uint Id { get; set; }
	public string Name { get; set; } = "";
	public string Email { get; set; } = "";
	public string PasswordHash { get; set; } = "";
	public bool Deactivated { get; set; }
	public bool EmailConfirmed { get; set; }
	public string Role { get; set; } = Roles.User;
}

public class UserDto(User user)
{
	public uint Id { get; set; } = user.Id;
	public string Name { get; set; } = user.Name;
	public string Email { get; set; } = user.Email;
	public bool Deactivated { get; set; } = user.Deactivated;
	public bool EmailConfirmed { get; set; } = user.EmailConfirmed;
	public string Role { get; set; } = user.Role;
}
