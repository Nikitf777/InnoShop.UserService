namespace InnoShop.UserService;

public class VariableNotFoundException(string message) : Exception(message);

public static class Env
{

#pragma warning disable CA1707 // Identifiers should not contain underscores

	public enum Vars
	{
		DB_HOST,
		DB_PORT,
		DB_NAME,
		DB_USER,
		DB_PASSWORD,
	}

#pragma warning restore CA1707 // Identifiers should not contain underscores

	public static string DbHost => GetVar(nameof(Vars.DB_HOST));
	public static string DbPort => GetVar(nameof(Vars.DB_PORT));
	public static string DbName => GetVar(nameof(Vars.DB_NAME));
	public static string DbUser => GetVar(nameof(Vars.DB_USER));
	public static string DbPassword => GetVar(nameof(Vars.DB_PASSWORD));

	private static string GetVar(string variable)
	{
		return Environment.GetEnvironmentVariable(variable) ?? throw new VariableNotFoundException($"Environment variable '{variable}' does not exist.");
	}
}