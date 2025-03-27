using EmployeeManager.API.Configuration;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace EmployeeManager.Application.Services
{
	public class LoginService(TokenConfiguration configuration, IUserRepository userRepository, ITokenService tokenService, IEmployeeService employeeService) : ILoginService
	{
		private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

		private readonly IUserRepository _userRepository = userRepository;
		private readonly ITokenService _tokenService = tokenService;
		private readonly IEmployeeService _employeeService = employeeService;
		private readonly TokenConfiguration _configuration = configuration;

		/// <summary>
		/// Validates the user credentials and returns a token if the credentials are valid
		/// </summary>
		/// <param name="userCredentials"></param>
		/// <returns>TokenDTO?</returns>
		public TokenDTO? ValidateCredentials(UserDTO userCredentials)
		{
			var userConvert = new User(userCredentials.UserDocument, userCredentials.Password, string.Empty);

			var user = _userRepository.ValidateCredentials(userConvert);

			if (user is null)
			{
				return null;
			}

			// Checks if the employee exists and is active for login
			var employee = _employeeService.GetEmployeeByDocument(user.UserDocument).GetAwaiter().GetResult();
			if (employee is null || !employee.Active)
			{
				return null;
			}

			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
				new(JwtRegisteredClaimNames.UniqueName, user.UserDocument)
			};

			var accessToken = _tokenService.GenerateAccessToken(claims);
			var refreshToken = _tokenService.GenerateRefreshToken();

			return UpdateUserToken(accessToken, refreshToken, user);
		}

		/// <summary>
		/// Validates the token and returns the token if it is valid
		/// </summary>
		/// <param name="token"></param>
		/// <returns>TokenDTO?</returns>
		public TokenDTO? ValidateCredentials(TokenDTO token)
		{
			var accessToken = token.AccessToken;
			var refreshToken = token.RefreshToken;

			var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

			var userName = principal.Identity?.Name;
			var user = _userRepository.ValidateCredentials(userName ?? string.Empty);

			if (user == null
				|| user.RefreshToken != refreshToken
				|| user.RefreshTokenExpiryTime <= DateTime.Now)
			{
				return null;
			}

			accessToken = _tokenService.GenerateAccessToken(principal.Claims);
			refreshToken = _tokenService.GenerateRefreshToken();

			return UpdateUserToken(accessToken, refreshToken, user);
		}

		/// <summary>
		/// Revokes the token for the given user
		/// </summary>
		/// <param name="userName"></param>
		/// <returns>bool</returns>
		public bool RevokeToken(string userName)
		{
			return _userRepository.RevokeToken(userName);
		}

		#region PRIVATE

		private TokenDTO UpdateUserToken(string accessToken, string refreshToken, User user)
		{
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpire);
			_userRepository.RefreshUserInfo(user);

			var createDate = DateTime.Now;
			var expirationDate = createDate.AddMinutes(_configuration.Minutes);

			return new TokenDTO(true, createDate.ToString(DATE_FORMAT), expirationDate.ToString(DATE_FORMAT), accessToken, refreshToken);
		}

		#endregion
	}
}
