using EmployeeManager.Domain.Models;

namespace EmployeeManager.Application.DTOs
{
	public class PhoneNumberDTO : BaseDTO
	{
		public string Number { get; set; }

		public long? EmployeeId { get; set; }

		public PhoneNumberDTO() { }

		public PhoneNumberDTO(PhoneNumber phoneNumber)
		{
			Id = phoneNumber.Id;
			Number = phoneNumber.Number;
			EmployeeId = phoneNumber.EmployeeId;
		}
	}
}
