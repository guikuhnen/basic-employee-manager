using EmployeeManager.Application.DTOs;
using EmployeeManager.Domain.Models;

namespace EmployeeManager.Application.Converter
{
	internal class EmployeeConverter : IParser<Employee, EmployeeDTO>, IParser<EmployeeDTO, Employee>
	{
		#region Model to DTO

		#endregion

		#region DTO to Model

		public EmployeeDTO Parse(Employee origin)
		{
			try
			{
				return new EmployeeDTO(origin);
				{
					Id = origin.Id,
					FirstName = origin.FirstName,
					LastName = origin.LastName,
					Email = origin.Email,
					DocumentNumber = origin.DocumentNumber,
					PhoneNumbers = origin.PhoneNumbers?.Select(p => new PhoneNumberDTO(p)).ToList(),
					ManagerId = origin.ManagerId,
					Role = origin.Role,
					Password = origin.Password,
					BirthDate = origin.BirthDate,
					Active = origin.Active
				};
			}
			catch (Exception e)
			{
				throw new Exception($"Error while converting EmployeeDTO to Employee - {e.Message}");
			}
		}

		public IEnumerable<EmployeeDTO> Parse(ICollection<Employee> origin)
		{
			try
			{
				return origin.Select(Parse);
			}
			catch (Exception e)
			{
				throw new Exception($"Error while converting many EmployeeDTO to Employee - {e.Message}");
			}
		}

		#endregion

	}
}
