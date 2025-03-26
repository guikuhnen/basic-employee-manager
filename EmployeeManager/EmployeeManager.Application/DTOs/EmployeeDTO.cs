using EmployeeManager.Domain.Enums;
using EmployeeManager.Domain.Models;

namespace EmployeeManager.Application.DTOs
{
	public class EmployeeDTO : BaseDTO
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string DocumentNumber { get; set; }
		public List<PhoneNumberDTO>? PhoneNumbers { get; set; }
		public long? ManagerId { get; set; }
		public ERoleType Role { get; set; }
		public string Password { get; set; }
		public DateTime BirthDate { get; set; }
		public bool Active { get; set; }

		#region CUSTOM

		public string Name { get; set; }

		#endregion

		public EmployeeDTO(Employee employee)
		{
			Id = employee.Id;
			FirstName = employee.FirstName;
			LastName = employee.LastName;
			Email = employee.Email;
			DocumentNumber = employee.DocumentNumber;
			PhoneNumbers = employee.PhoneNumbers?.Select(p => new PhoneNumberDTO(p)).ToList();
			ManagerId = employee.ManagerId;
			Role = employee.Role;
			Password = employee.Password;
			BirthDate = employee.BirthDate;
			Active = employee.Active;

			#region CUSTOM

			Name = employee.FirstName + " " + employee.LastName;

			#endregion
		}
	}
}
