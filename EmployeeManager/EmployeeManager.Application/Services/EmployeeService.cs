using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Services
{
	internal class EmployeeService(ILogger<EmployeeService> logger,
		IEmployeeRepository repository,
		IPhoneNumberRepository phoneNumberRepository) : BaseService<EmployeeService>(logger), IEmployeeService
	{
		public readonly IEmployeeRepository _repository = repository;
		public readonly IPhoneNumberRepository _phoneNumberRepository = phoneNumberRepository;

		/// <summary>
		/// Add an employee to the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		public async Task AddEmployee(EmployeeDTO employee)
		{
			try
			{
				var newEmployee = await ConvertEmployeeDTODomain(employee, false);

				await _repository.AddEmployee(newEmployee);

				if (employee.PhoneNumbers != null)
				{
					List<PhoneNumber> phoneNumbers = employee.PhoneNumbers.Select(p => new PhoneNumber(p.Number, newEmployee)).ToList();

					await _phoneNumberRepository.AddPhoneNumbers(phoneNumbers);
				}
			}
			catch (Exception e)
			{
				var errorMessage = $"AddEmployee - ERROR - {e.Message}";
				_logger.LogError("{Message}", errorMessage);
				throw;
			}
		}

		/// <summary>
		/// Get all employees from the database
		/// </summary>
		/// <returns>IEnumerable<EmployeeDTO></returns>
		public async Task<IEnumerable<EmployeeDTO>?> GetAllEmployees()
		{
			try
			{
				var employees = await _repository.GetAllEmployees();

				return employees?.Select(e => new EmployeeDTO(e)).ToList();
			}
			catch (Exception e)
			{
				var errorMessage = $"GetAllEmployees - ERROR - {e.Message}";
				_logger.LogError("{Message}", errorMessage);
				throw;
			}
		}

		/// <summary>
		/// Get an employee by their ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>EmployeeDTO</returns>
		public async Task<EmployeeDTO> GetEmployee(long id)
		{
			try
			{
				var employee = await _repository.GetEmployee(id);

				return new EmployeeDTO(employee);
			}
			catch (Exception e)
			{
				var errorMessage = $"GetEmployee - ERROR - {e.Message}";
				_logger.LogError("{Message}", errorMessage);
				throw;
			}
		}

		/// <summary>
		/// Update an employee in the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		public async Task UpdateEmployee(EmployeeDTO employee)
		{
			try
			{
				var updateEmployee = await ConvertEmployeeDTODomain(employee, true);

				await _repository.UpdateEmployee(updateEmployee);

				if (updateEmployee.PhoneNumbers?.Count > 0)
				{
					await _phoneNumberRepository.UpdatePhoneNumbers(updateEmployee.PhoneNumbers);
				}
			}
			catch (Exception e)
			{
				var errorMessage = $"UpdateEmployee - ERROR - {e.Message}";
				_logger.LogError("{Message}", errorMessage);
				throw;
			}
		}

		/// <summary>
		/// Delete (active = false) an employee from the database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task DeleteEmployee(long id)
		{
			try
			{
				await _repository.DeleteEmployee(id);
			}
			catch (Exception e)
			{
				var errorMessage = $"DeleteEmployee - ERROR - {e.Message}";
				_logger.LogError("{Message}", errorMessage);
				throw;
			}
		}

		#region PRIVATE

		private async Task<Employee> ConvertEmployeeDTODomain(EmployeeDTO employee, bool informId)
		{
			var manager = employee.ManagerId.HasValue ? await _repository.GetEmployee(employee.ManagerId.Value) : null;

			var newEmployee = new Employee
			(
				employee.FirstName,
				employee.LastName,
				employee.Email,
				employee.DocumentNumber,
				null,
				manager,
				employee.Role,
				employee.Password,
				employee.BirthDate,
				employee.Active
			);

			if (informId)
			{
				newEmployee.Id = employee.Id;
				newEmployee.PhoneNumbers = employee.PhoneNumbers?.Select(p => new PhoneNumber(p.Number, newEmployee)).ToList();
			}

			return newEmployee;
		}

		#endregion
	}
}
