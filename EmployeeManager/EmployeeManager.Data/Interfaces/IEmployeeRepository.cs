using EmployeeManager.Domain.Models;

namespace EmployeeManager.Data.Interfaces
{
	public interface IEmployeeRepository
	{
		/// <summary>
		/// Add an employee to the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		Task AddEmployee(Employee employee);

		/// <summary>
		/// Get all employees from the database
		/// </summary>
		/// <returns>IEnumerable<Employee></returns>
		Task<IEnumerable<Employee>> GetAllEmployees();

		/// <summary>
		/// Get an employee by their ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Employee</returns>
		Task<Employee> GetEmployee(long id);

		/// <summary>
		/// Update an employee in the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		Task UpdateEmployee(Employee employee);

		/// <summary>
		/// Delete (active = false) an employee from the database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task DeleteEmployee(long id);
	}
}
