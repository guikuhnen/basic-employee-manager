﻿using EmployeeManager.Data.Context;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Data.Repositories
{
	public class EmployeeRepository(MySQLContext context) : IEmployeeRepository
	{
		public readonly MySQLContext _context = context;

		/// <summary>
		/// Add an employee to the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		public async Task AddEmployee(Employee employee)
		{
			_context.Add(employee);

			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Get all employees from the database
		/// </summary>
		/// <returns>IEnumerable<Employee></returns>
		public async Task<IEnumerable<Employee>> GetAllEmployees()
		{
			return await _context.Employees
				.Include(e => e.PhoneNumbers)
				.ToListAsync();
		}

		/// <summary>
		/// Get an employee by their ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Employee</returns>
		public async Task<Employee> GetEmployee(long id)
		{
			return await FindEmployeeById(id);
		}

		/// <summary>
		/// Update an employee in the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		public async Task UpdateEmployee(Employee employee)
		{
			_context.Update(employee);

			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Delete (active = false) an employee from the database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task DeleteEmployee(long id)
		{
			var employee = await FindEmployeeById(id);

			if (employee is not null)
			{
				employee.Active = false;

				_context.Update(employee);

				await _context.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Get an employee by their document number
		/// </summary>
		/// <param name="document"></param>
		/// <returns>Employee</returns>
		public async Task<Employee> GetEmployeeByDocument(string document)
		{
			var employee = await _context.Employees
				.Include(p => p.PhoneNumbers)
				.Where(e => e.DocumentNumber.Equals(document))
				.FirstOrDefaultAsync();

			if (employee is not null)
			{
				return employee;
			}
			else
			{
				throw new Exception("Employee not found.");
			}
		}

		#region PRIVATE

		private async Task<Employee> FindEmployeeById(long id)
		{
			var employee = await _context.Employees
				.Include(p => p.PhoneNumbers)
				.Where(e => e.Id.Equals(id))
				.FirstOrDefaultAsync();

			if (employee is not null)
			{
				return employee;
			}
			else
			{
				throw new Exception("Employee not found.");
			}
		}

		#endregion
	}
}
