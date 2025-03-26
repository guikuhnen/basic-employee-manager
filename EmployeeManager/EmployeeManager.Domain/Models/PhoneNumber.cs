using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Domain.Models
{
	public class PhoneNumber : BaseEntity
	{
		[DataType(DataType.PhoneNumber), StringLength(20, ErrorMessage = "Enter a valid phone number.")]
		public required string Number { get; set; }

		[ForeignKey("EmployeeId")]
		public required Employee Employee { get; set; }
		public required long EmployeeId { get; set; }

		public PhoneNumber() { }

		public PhoneNumber(string number, Employee employee)
		{
			Number = number;
			Employee = employee;
		}
	}
}