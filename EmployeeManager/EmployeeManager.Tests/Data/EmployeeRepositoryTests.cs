using EmployeeManager.Data.Context;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Domain.Models;
using EmployeeManager.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace EmployeeManager.Tests.Data
{
	public class EmployeeRepositoryTests
	{
		private readonly EmployeeRepository _repository;
		private readonly DbContextOptions<MySQLContext> _options;
		private readonly MySQLContext _context;
		private readonly DataMocks _employees;

		public EmployeeRepositoryTests()
		{
			_options = new DbContextOptionsBuilder<MySQLContext>()
				.UseInMemoryDatabase(databaseName: "EmployeeDatabase")
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

			_repository = new EmployeeRepository(_context);
		}

		[Fact]
		public void Test_AddEmployee_Execution_Completes()
		{
			//Arrange
			var options = new DbContextOptionsBuilder<MySQLContext>()
				.UseInMemoryDatabase(databaseName: "EmployeeDatabase2")
				.Options;

			var employee = new DataMocks();
			employee.EmployeePresident.Id = 1;

			using var context = new MySQLContext(options);
			var repository = new EmployeeRepository(context);
		
			//Act
			var result = repository.AddEmployee(employee.EmployeePresident);
		
			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
		}

		[Fact]
		public async void Test_GetAllEmployees_Execution_Completes()
		{
			//Arrange
			//Act
			var result = await _repository.GetAllEmployees();

			//Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async void Test_GetEmployee_Execution_Completes()
		{
			//Act
			var result = await _repository.GetEmployee(1);

			//Assert
			Assert.NotNull(result);
			Assert.Equal(_employees.EmployeePresident, result);
		}

		[Fact]
		public void Test_GetEmployee_Throws_Exception()
		{
			//Arrange
			//Act
			var result = _repository.GetEmployee(3);
		
			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}
		
		[Fact]
		public async void Test_UpdateEmployee_Execution_Completes()
		{
			//Arrange
			//Act
			_employees.EmployeePresident.FirstName = "Teste 3";
			await _repository.UpdateEmployee(_employees.EmployeePresident);
			var result = await _repository.GetEmployee(1);
		
			//Assert
			Assert.NotNull(result);
			Assert.Equal("Teste 3", result.FirstName);
		}
		
		[Fact]
		public async void Test_DeleteEmployee_Execution_Completes()
		{
			//Arrange
			//Act
			await _repository.DeleteEmployee(1);
			var result = await _repository.GetEmployee(1);
		
			//Assert
			Assert.NotNull(result);
			Assert.False(result.Active);
		}
		
		[Fact]
		public async void Test_GetEmployeeByDocument_Execution_Completes()
		{
			//Arrange
			//Act
			var result = await _repository.GetEmployeeByDocument("12345678903");
		
			//Assert
			Assert.NotNull(result);
			Assert.Equal(_employees.EmployeeDirector.Id, result.Id);
		}

		[Fact]
		public void Test_GetEmployeeByDocument_Throws_Exception()
		{
			//Arrange
			//Act
			var result = _repository.GetEmployeeByDocument("1234");

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}
	}
}
