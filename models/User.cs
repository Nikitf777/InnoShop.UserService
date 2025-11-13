namespace InnoShop.UserService.Models;

public enum Role
{
	Admin,
	User,
}

public class User
{
	public int Id { get; set; }
	public string Name { get; set; } = "";
	public string Email { get; set; } = "";
	public string PasswordHash { get; set; } = "";
	public List<string> Roles { get; set; } = [];
}
