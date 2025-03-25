using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Domain.Models
{
	internal class PhoneNumber : BaseEntity
	{
		[DataType(DataType.PhoneNumber), Phone(ErrorMessage = "Enter a valid phone number."), MaxLength(20)]
		public required string Number { get; set; }

		[ForeignKey("EmployeeId")]
		public required Employee Employee { get; set; }
		public required long EmployeeId { get; set; }
	}
}