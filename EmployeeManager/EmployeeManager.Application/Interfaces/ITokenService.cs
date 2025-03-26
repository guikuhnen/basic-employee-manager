using System.Security.Claims;

namespace EmployeeManager.Application.Interfaces
{
	public interface ITokenService
	{
		/// <summary>
		/// Generates an access token for the given claims
		/// </summary>
		/// <param name="claims"></param>
		/// <returns>string</returns>
		string GenerateAccessToken(IEnumerable<Claim> claims);

		/// <summary>
		/// Generates a refresh token
		/// </summary>
		/// <returns>string</returns>
		string GenerateRefreshToken();

		/// <summary>
		/// Validates the given token and returns the claims if the token is valid
		/// </summary>
		/// <param name="token"></param>
		/// <returns>ClaimsPrincipal</returns>
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}
