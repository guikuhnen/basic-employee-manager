using EmployeeManager.Application.DTOs;

namespace EmployeeManager.Application.Interfaces
{
	public interface IEmployeeService
	{
		/// <summary>
		/// Add an employee to the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		Task AddEmployee(EmployeeDTO employee);

		/// <summary>
		/// Get all employees from the database
		/// </summary>
		/// <returns>IEnumerable<EmployeeDTO></returns>
		Task<IEnumerable<EmployeeDTO>> GetAllEmployees();

		/// <summary>
		/// Get an employee by their ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>EmployeeDTO</returns>
		Task<EmployeeDTO> GetEmployee(long id);

		/// <summary>
		/// Update an employee in the database
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		Task UpdateEmployee(EmployeeDTO employee);

		/// <summary>
		/// Delete (active = false) an employee from the database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task DeleteEmployee(long id);
	}
}
