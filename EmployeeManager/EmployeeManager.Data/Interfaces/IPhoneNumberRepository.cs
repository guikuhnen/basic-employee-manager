using EmployeeManager.Domain.Models;

namespace EmployeeManager.Data.Interfaces
{
	public interface IPhoneNumberRepository
	{
		/// <summary>
		/// Add many phone numbers to the database
		/// </summary>
		/// <param name="phoneNumbers"></param>
		/// <returns></returns>
		Task AddPhoneNumbers(List<PhoneNumber> phoneNumbers);

		/// <summary>
		/// Get all phone numbers of the employee from the database
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns>IEnumerable<PhoneNumber></returns>
		Task<IEnumerable<PhoneNumber>?> GetAllPhoneNumbersByEmployeeId(long employeeId);

		/// <summary>
		/// Delete many phone numbers from the database by employee id
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns></returns>
		Task DeletePhoneNumbersByEmployeeId(long employeeId);
	}
}
