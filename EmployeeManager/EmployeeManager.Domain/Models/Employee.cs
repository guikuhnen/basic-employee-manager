using EmployeeManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EmployeeManager.Domain.Models
{
	[Table("Employees")]
	public class Employee : BaseEntity
	{
		[Display(Name = "First Name"), StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required string FirstName { get; set; }

		[Display(Name = "Last Name"), StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required string LastName { get; set; }

		[DataType(DataType.EmailAddress), EmailAddress(ErrorMessage = "Enter a valid e-mail."), MaxLength(40)]
		public required string Email { get; set; }

		[Display(Name = "Document Number"), StringLength(20, MinimumLength = 6, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required string DocumentNumber { get; set; }

		public List<PhoneNumber>? PhoneNumbers { get; set; }

		[ForeignKey("ManagerId")]
		public virtual Employee? Manager { get; set; }
		public long? ManagerId { get; set; }

		[Display(Name ="Role")]
		public required ERoleType Role { get; set; }

		[Display(Name = "Birth Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"), Required(AllowEmptyStrings = false, ErrorMessage = "Inform {0}.")]
		public required DateTime BirthDate { get; set; }

		public required bool Active { get; set; }

		public Employee() { }

		[SetsRequiredMembers]
		public Employee(string firstName, string lastName, string email, string documentNumber, List<PhoneNumber>? phoneNumbers, Employee? manager, ERoleType role, DateTime birthDate, bool active)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			DocumentNumber = documentNumber;
			PhoneNumbers = phoneNumbers;
			Manager = manager;
			Role = role;
			BirthDate = birthDate;
			Active = active;
		}

		public bool IsMinor()
		{
			return BirthDate.AddYears(18).Date > DateTime.Now.Date;
		}

		public bool CanBeManager(ERoleType employeeRole)
		{
			return (int)employeeRole >= (int)Role;
		}
	}
}
