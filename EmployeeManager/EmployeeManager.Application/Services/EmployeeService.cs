using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Services
{
	internal class EmployeeService : BaseService, IEmployeeService
	{
		public readonly IEmployeeRepository _repository;

		public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository repository) : base(logger)
		{
			_repository = repository;
		}

		/// <summary>
		/// Add an employee to the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		public async Task AddEmployee(EmployeeDTO employee)
		{
			try
			{
				var phoneNumbers = _repository

				var newEmployee = new Employee
				(
					employee.FirstName,
					employee.LastName,
					employee.Email,
					employee.DocumentNumber,
					employee.PhoneNumbers?.Select(p => new PhoneNumber(p.Number, p.EmployeeId)).ToList(),
					employee.ManagerId,
					employee.Role,
					employee.Password,
					employee.BirthDate,
					employee.Active
				);

				await _repository.AddEmployee(newEmployee);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"AddEmployee - ERROR - {e.Message}");
				throw e;
			}
		}

		/// <summary>
		/// Get all employees from the database
		/// </summary>
		/// <returns>IEnumerable<EmployeeDTO></returns>
		public Task<IEnumerable<EmployeeDTO>> GetAllEmployees()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get an employee by their ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>EmployeeDTO</returns>
		public Task<EmployeeDTO> GetEmployee(long id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Update an employee in the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		public Task UpdateEmployee(EmployeeDTO employee)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Delete (active = false) an employee from the database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task DeleteEmployee(long id)
		{
			throw new NotImplementedException();
		}
	}
}
