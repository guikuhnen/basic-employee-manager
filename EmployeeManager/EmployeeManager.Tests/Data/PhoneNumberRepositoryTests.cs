using EmployeeManager.Data.Context;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Domain.Models;
using EmployeeManager.Tests.Utils;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Tests.Data
{
	public class PhoneNumberRepositoryTests
	{
		private readonly PhoneNumberRepository _repository;
		private readonly DbContextOptions<MySQLContext> _options;
		private readonly MySQLContext _context;
		private readonly DataMocks _employees;
		public PhoneNumberRepositoryTests()
		{
			_options = new DbContextOptionsBuilder<MySQLContext>()
				.UseInMemoryDatabase(databaseName: "PhoneNumberDatabase")
				.Options;

			_employees = new DataMocks();
			_employees.EmployeePresident.Id = 1;
			_employees.EmployeeDirector.Id = 2;

			_context = new MySQLContext(_options);
			if (_context.Employees.Count() < 2)
			{
				_context.Employees.Add(_employees.EmployeePresident);
				_context.Employees.Add(_employees.EmployeeDirector);
				_context.SaveChanges();
			}

			_repository = new PhoneNumberRepository(_context);
		}

		[Fact]
		public void Test_AddPhoneNumbers_Execution_Completes()
		{
			//Arrange
			var phoneNumbers = new List<PhoneNumber>
			{
				new("47991075598", _employees.EmployeeDirector)
			};
			//Act
			var result = _repository.AddPhoneNumbers(phoneNumbers);

			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
			Assert.Equal(2, _context.PhoneNumbers.Where(x => x.EmployeeId.Equals(_employees.EmployeeDirector.Id)).Count());
		}

		[Fact]
		public void Test_GetAllPhoneNumbersByEmployeeId_Execution_Completes()
		{
			//Arrange
			//Act
			var result = _repository.GetAllPhoneNumbersByEmployeeId(_employees.EmployeePresident.Id);

			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
			Assert.Equal(2, result?.Result?.Count());
		}

		[Fact]
		public async void Test_DeletePhoneNumbersByEmployeeId_Execution_Completes()
		{
			//Arrange
			var options = new DbContextOptionsBuilder<MySQLContext>()
				.UseInMemoryDatabase(databaseName: "PhoneNumberDatabase2")
				.Options;

			using var context = new MySQLContext(options);
			context.Employees.Add(_employees.EmployeePresident);
			context.Employees.Add(_employees.EmployeeDirector);
			context.SaveChanges();
			var repository = new PhoneNumberRepository(context);

			//Act
			//Act
			await repository.DeletePhoneNumbersByEmployeeId(_employees.EmployeePresident.Id);

			//Assert
			Assert.Equal(0, context.PhoneNumbers.Where(x => x.EmployeeId.Equals(_employees.EmployeePresident.Id)).Count());
		}
	}
}
