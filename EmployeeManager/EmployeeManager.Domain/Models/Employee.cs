using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManager.Domain.Models
{
	[Table("Employees")]
	internal class Employee : BaseEntity
	{
		[Display(Name = "First Name"), StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required string FirstName { get; set; }

		[Display(Name = "Last Name"), StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required string LastName { get; set; }

		[DataType(DataType.EmailAddress), EmailAddress(ErrorMessage = "Enter a valid e-mail.")]
		public required string Email { get; set; }

		[Key, DatabaseGenerated(DatabaseGeneratedOption.None), Display(Name = "Document Number"), Length(6, 20, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required int DocumentNumber { get; set; }

		// Manager
		// Role

		[DataType(DataType.Password)]
		public required string Password { get; set; }

		public string? RefreshToken { get; set; }

		public DateTime RefreshTokenExpiryTime { get; set; }

		[Display(Name = "Birth Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"), Required(AllowEmptyStrings = false, ErrorMessage = "Inform {0}.")]
		public required DateTime BirthDate { get; set; }

		// Validate if is a minor
	}
}
