using EmployeeManager.API.Configuration;
using EmployeeManager.Application.Services;
using EmployeeManager.Data.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManager.Tests.Application
{
	public class TokenServiceTests
	{
		private readonly TokenConfiguration _tokenConfigurationMock = new TokenConfiguration()
		{
			Audience = "Audience",
			Issuer = "Issuer",
			Secret = "MY_NEWEST_SUPER_SUPER_SECRET_KEYMY_NEWEST_SUPER_SUPER_SECRET_KEY",
			Minutes = 60,
			DaysToExpire = 7
		};
		private readonly TokenService _service;

		public TokenServiceTests()
		{
			_service = new(_tokenConfigurationMock);
		}

		[Fact]
		public void Test_GenerateAccessToken_ShouldReturnToken()
		{
			// Arrange
			var claims = new List<Claim>
			{
				new(ClaimTypes.Name, "Test"),
				new(ClaimTypes.Role, "Admin")
			};

			// Act
			var token = _service.GenerateAccessToken(claims);

			// Assert
			Assert.NotNull(token);
			Assert.NotEmpty(token);
		}

		[Fact]
		public void Test_GenerateRefreshToken_ShouldReturnToken()
		{
			// Arrange
			// Act
			var token = _service.GenerateRefreshToken();

			// Assert
			Assert.NotNull(token);
			Assert.NotEmpty(token);
		}

		[Fact]
		public void Test_GetPrincipalFromExpiredToken_ShouldReturnClaimsPrincipal()
		{
			// Arrange
			var claims = new List<Claim>
			{
				new(ClaimTypes.Name, "Test"),
				new(ClaimTypes.Role, "Admin")
			};

			// Act
			var token = _service.GenerateAccessToken(claims);
			var principal = _service.GetPrincipalFromExpiredToken(token);

			// Assert
			Assert.NotNull(principal);
			Assert.NotEmpty(principal.Claims);
		}

		[Fact]
		public void Test_GetPrincipalFromExpiredToken_Throws_Exception()
		{
			// Arrange
			var claims = new List<Claim>
			{
				new(ClaimTypes.Name, "Test"),
				new(ClaimTypes.Role, "Admin")
			};
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfigurationMock.Secret));
			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha384);

			var tokenOptions = new JwtSecurityToken(
				issuer: _tokenConfigurationMock.Issuer,
				audience: _tokenConfigurationMock.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_tokenConfigurationMock.Minutes),
				signingCredentials: signinCredentials
			);

			// Generate HmacSha384 Token to difer from default HmacSha256
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			// Act
			// Assert
			Assert.Throws<SecurityTokenException>(() => _service.GetPrincipalFromExpiredToken(token));
		}
	}
}
