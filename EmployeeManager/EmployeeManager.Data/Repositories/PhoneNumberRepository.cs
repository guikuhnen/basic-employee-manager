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
			await CreatePhoneNumbers(phoneNumbers);
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
		/// Update many phone numbers in the database
		/// </summary>
		/// <param name="phoneNumbers"></param>
		/// <returns></returns>
		public async Task UpdatePhoneNumbers(List<PhoneNumber> phoneNumbers)
		{
			var phonesToRemove = await FindPhoneNumbersByEmployeeId(phoneNumbers.First().EmployeeId);

			if (phonesToRemove is not null)
			{
				_context.RemoveRange(phonesToRemove);
			}

			await CreatePhoneNumbers(phoneNumbers);
		}

		#region PRIVATE

		private async Task CreatePhoneNumbers(List<PhoneNumber> phoneNumbers)
		{
			await _context.AddRangeAsync(phoneNumbers);

			await _context.SaveChangesAsync();
		}

		private async Task<IEnumerable<PhoneNumber>?> FindPhoneNumbersByEmployeeId(long employeeId)
		{
			return await _context.PhoneNumbers.Where(p => p.EmployeeId == employeeId).ToListAsync();
		}

		#endregion
	}
}
