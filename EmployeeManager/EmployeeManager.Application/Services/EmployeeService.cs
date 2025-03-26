﻿using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Services
{
	public class EmployeeService(ILogger<EmployeeService> logger,
		IEmployeeRepository repository,
		IPhoneNumberRepository phoneNumberRepository) : IEmployeeService
	{
		private readonly ILogger<EmployeeService> _logger = logger;
		private readonly IEmployeeRepository _repository = repository;
		private readonly IPhoneNumberRepository _phoneNumberRepository = phoneNumberRepository;

		/// <summary>
		/// Add an employee to the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		public async Task AddEmployee(EmployeeDTO employee)
		{
			try
			{
				_logger.LogInformation("EmployeeService - AddEmployee - Begin");

				var newEmployee = await ConvertEmployeeDTODomain(employee, false);

				await _repository.AddEmployee(newEmployee);

				if (employee.PhoneNumbers != null)
				{
					_logger.LogInformation("EmployeeService - AddEmployee - PhoneNumbers");

					List<PhoneNumber> phoneNumbers = employee.PhoneNumbers.Select(p => new PhoneNumber(p.Number, newEmployee)).ToList();

					await _phoneNumberRepository.AddPhoneNumbers(phoneNumbers);
				}

				_logger.LogInformation("EmployeeService - AddEmployee - End");
			}
			catch (Exception e)
			{
				var errorMessage = $"EmployeeService - AddEmployee - ERROR - {e.Message}";
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
				_logger.LogInformation("EmployeeService - GetAllEmployees - Begin");

				var employees = await _repository.GetAllEmployees();

				var result = employees?.Select(e => new EmployeeDTO(e)).ToList();

				_logger.LogInformation("EmployeeService - GetAllEmployees - End");

				return result;
			}
			catch (Exception e)
			{
				var errorMessage = $"EmployeeService - GetAllEmployees - ERROR - {e.Message}";
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
				_logger.LogInformation("EmployeeService - GetEmployee - Begin");

				var employee = await _repository.GetEmployee(id);

				var result = new EmployeeDTO(employee);

				_logger.LogInformation("EmployeeService - GetEmployee - End");

				return result;
			}
			catch (Exception e)
			{
				var errorMessage = $"EmployeeService - GetEmployee - ERROR - {e.Message}";
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
				_logger.LogInformation("EmployeeService - UpdateEmployee - Begin");

				var updateEmployee = await ConvertEmployeeDTODomain(employee, true);

				await _repository.UpdateEmployee(updateEmployee);

				if (updateEmployee.PhoneNumbers?.Count > 0)
				{
					_logger.LogInformation("EmployeeService - UpdateEmployee - PhoneNumbers");

					await _phoneNumberRepository.UpdatePhoneNumbers(updateEmployee.PhoneNumbers);
				}

				_logger.LogInformation("EmployeeService - UpdateEmployee - End");
			}
			catch (Exception e)
			{
				var errorMessage = $"EmployeeService - UpdateEmployee - ERROR - {e.Message}";
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
				_logger.LogInformation("EmployeeService - DeleteEmployee - Begin");

				await _repository.DeleteEmployee(id);

				_logger.LogInformation("EmployeeService - DeleteEmployee - End");
			}
			catch (Exception e)
			{
				var errorMessage = $"EmployeeService - DeleteEmployee - ERROR - {e.Message}";
				_logger.LogError("{Message}", errorMessage);
				throw;
			}
		}

		#region PRIVATE

		private async Task<Employee> ConvertEmployeeDTODomain(EmployeeDTO employee, bool informId)
		{
			var manager = employee.ManagerId.HasValue ? await _repository.GetEmployee(employee.ManagerId.Value) : null;

			if (manager is not null && !manager.CanBeManager(employee.Role))
			{
				throw new Exception("The employee selected to act like a manager is invalid.");
			}

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

			if (newEmployee.IsMinor())
			{
				throw new Exception("This person is a minor. Please register a valid employee. ");
			}

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
