using EmployeeManager.Data.Context;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Data.Repositories
{
	public class PhoneNumberRepository(MySQLContext context) : IPhoneNumberRepository
	{
		public readonly MySQLContext _context = context;

		/// <summary>
		/// Add many phone numbers to the database
		/// </summary>
		/// <param name="phoneNumbers"></param>
		/// <returns></returns>
		public async Task AddPhoneNumbers(List<PhoneNumber> phoneNumbers)
		{
			await _context.AddRangeAsync(phoneNumbers);

			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Get all phone numbers of the employee from the database
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns>IEnumerable<PhoneNumber></returns>
		public async Task<IEnumerable<PhoneNumber>?> GetAllPhoneNumbersByEmployeeId(long employeeId)
		{
			return await FindPhoneNumbersByEmployeeId(employeeId);
		}

		/// <summary>
		/// Delete many phone numbers from the database by employee id
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns></returns>
		public async Task DeletePhoneNumbersByEmployeeId(long employeeId)
		{
			var phonesToRemove = await FindPhoneNumbersByEmployeeId(employeeId);

			if (phonesToRemove?.Count() > 0)
			{
				_context.RemoveRange(phonesToRemove);
			}

			await _context.SaveChangesAsync();
		}

		#region PRIVATE

		private async Task<IEnumerable<PhoneNumber>?> FindPhoneNumbersByEmployeeId(long employeeId)
		{
			return await _context.PhoneNumbers.Where(p => p.EmployeeId == employeeId).ToListAsync();
		}

		#endregion
	}
}
