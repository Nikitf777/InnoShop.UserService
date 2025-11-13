using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace InnoShop.UserService.Services;

public static class PasswordHasher
{
	private const int SaltSize = 16;
	private const int KeySize = 32;
	private const int Iterations = 350000;

	private const char SegmentDelimiter = ':';

	public static bool VerifyPassword(string password, string hashedPassword)
	{
		if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword)) {
			return false;
		}

		var segments = hashedPassword.Split(SegmentDelimiter);
		if (segments.Length != 3) {
			return false;
		}

		if (!int.TryParse(segments[0], out var storedIterations)) {
			return false;
		}

		var salt = Convert.FromBase64String(segments[1]);
		var storedHash = Convert.FromBase64String(segments[2]);

		Span<byte> passwordBuffer = stackalloc byte[password.Length * sizeof(char)];
		password.CopyToUtf16Bytes(passwordBuffer);

		var computedHash = GenerateHash(passwordBuffer, salt, storedIterations, KeySize);

		return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
	}

	public static string HashPassword(string password)
	{
		if (string.IsNullOrWhiteSpace(password)) {
			throw new ArgumentException("Password cannot be empty.", nameof(password));
		}

		var salt = RandomNumberGenerator.GetBytes(SaltSize);

		Span<byte> passwordBuffer = stackalloc byte[password.Length * sizeof(char)];
		password.CopyToUtf16Bytes(passwordBuffer);
		var hash = GenerateHash(passwordBuffer, salt, Iterations, KeySize);

		return $"{Iterations}{SegmentDelimiter}{Convert.ToBase64String(salt)}{SegmentDelimiter}{Convert.ToBase64String(hash)}";
	}

	private static byte[] GenerateHash(ReadOnlySpan<byte> password, ReadOnlySpan<byte> salt, int iterations, int keyLength)
	{
		var deriveBytes = Rfc2898DeriveBytes.Pbkdf2(
			password,
			salt,
			iterations,
			HashAlgorithmName.SHA256,
			keyLength
		);
		return deriveBytes;
	}
}

public static class StringExtension
{
	public static void CopyToUtf16Bytes(this string str, Span<byte> destination)
	{
		var sourceBytes = MemoryMarshal.AsBytes(str.AsSpan());
		sourceBytes.CopyTo(destination);
	}
}