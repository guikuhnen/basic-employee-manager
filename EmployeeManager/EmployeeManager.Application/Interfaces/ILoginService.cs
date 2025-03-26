using EmployeeManager.Application.DTOs;

namespace EmployeeManager.Application.Interfaces
{
	public interface ILoginService
	{
		/// <summary>
		/// Validates the user credentials and returns a token if the credentials are valid
		/// </summary>
		/// <param name="userCredentials"></param>
		/// <returns>TokenDTO?</returns>
		TokenDTO? ValidateCredentials(UserDTO userCredentials);

		/// <summary>
		/// Validates the token and returns the token if it is valid
		/// </summary>
		/// <param name="token"></param>
		/// <returns>TokenDTO?</returns>
		TokenDTO? ValidateCredentials(TokenDTO token);

		/// <summary>
		/// Revokes the token for the given user
		/// </summary>
		/// <param name="userName"></param>
		/// <returns>bool</returns>
		bool RevokeToken(string userName);
	}
}
