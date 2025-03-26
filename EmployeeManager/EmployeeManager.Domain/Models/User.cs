using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EmployeeManager.Domain.Models
{
	[Table("Users")]
	public class User : BaseEntity
	{
		[Display(Name = "User Document"), StringLength(20, MinimumLength = 6, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required string UserDocument { get; set; }

		[DataType(DataType.Password)]
		public required string Password { get; set; }

		[Display(Name = "Full Name"), StringLength(120, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1} characters.")]
		public required string FullName { get; set; }

		public string? RefreshToken { get; set; }

		public DateTime? RefreshTokenExpiryTime { get; set; }

		public User() { }

		[SetsRequiredMembers]
		public User(string userDocument, string password, string fullName)
		{
			UserDocument = userDocument;
			Password = password;
			FullName = fullName;
		}
	}
}
