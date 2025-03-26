using EmployeeManager.Domain.Models;

namespace EmployeeManager.Data.Interfaces
{
	public interface IUserRepository
	{
		/// <summary>
		/// Validates the user credentials and returns the user if the credentials are valid
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		User? ValidateCredentials(User user);

		/// <summary>
		/// Validates the user credentials by the userDocument and returns the user if the credentials are valid
		/// </summary>
		/// <param name="userDocument"></param>
		/// <returns></returns>
		User? ValidateCredentials(string userDocument);

		/// <summary>
		/// Refreshes the user information
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		User? RefreshUserInfo(User user);

		/// <summary>
		/// Revokes the token for the given user
		/// </summary>
		/// <param name="userDocument"></param>
		/// <returns></returns>
		bool RevokeToken(string userDocument);
	}
}
