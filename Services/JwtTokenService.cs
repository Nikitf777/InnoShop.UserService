using System.Security.Claims;
using InnoShop.CommonEnvironment;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace InnoShop.UserService.Services;

public interface IJwtTokenService
{
	public string GenerateToken(string email);
}

public class JwtTokenService : IJwtTokenService
{
	public string GenerateToken(string email)
	{
		var handler = new JsonWebTokenHandler();

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.Sub, email),
			new(JwtRegisteredClaimNames.Email, email),
		};

		var tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(30),
			Issuer = Env.JwtKeyIssuer,
			Audience = Env.JwtKeyAudience,
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(Env.JwtSecretKeyBytes),
				SecurityAlgorithms.HmacSha256Signature)
		};

		return handler.CreateToken(tokenDescriptor);
	}
}