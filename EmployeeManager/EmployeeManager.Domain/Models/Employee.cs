using EmployeeManager.Domain.Enums;
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

		[DataType(DataType.EmailAddress), EmailAddress(ErrorMessage = "Enter a valid e-mail."), MaxLength(40)]
		public required string Email { get; set; }

		[Key, DatabaseGenerated(DatabaseGeneratedOption.None), Display(Name = "Document Number"), Length(6, 20, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required int DocumentNumber { get; set; }

		public required List<PhoneNumber> PhoneNumbers { get; set; }

		[ForeignKey("ManagerId")]
		public virtual Employee? Manager { get; set; }
		public long? ManagerId { get; set; }

		[Display(Name ="Role")]
		public required ERoleType Role { get; set; }

		[DataType(DataType.Password)]
		public required string Password { get; set; }

		[Display(Name = "Birth Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"), Required(AllowEmptyStrings = false, ErrorMessage = "Inform {0}.")]
		public required DateTime BirthDate { get; set; }

		public required bool Active { get; set; }

		public string? RefreshToken { get; set; }

		public DateTime? RefreshTokenExpiryTime { get; set; }

		public Employee() { }

		public Employee(string firstName, string lastName, string email, int documentNumber, List<PhoneNumber> phoneNumbers, Employee? manager, ERoleType role, string password, DateTime birthDate, bool active, string? refreshToken, DateTime? refreshTokenExpiryTime)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			DocumentNumber = documentNumber;
			PhoneNumbers = phoneNumbers;
			Manager = manager;
			Role = role;
			Password = password;
			BirthDate = birthDate;
			Active = active;
			RefreshToken = refreshToken;
			RefreshTokenExpiryTime = refreshTokenExpiryTime;
		}

		public bool IsMinor()
		{
			return BirthDate.AddYears(18) > DateTime.Now;
		}

		public bool CanBeManager(ERoleType toBeManagerRole)
		{
			return (int)toBeManagerRole <= (int)Role;
		}
	}
}
