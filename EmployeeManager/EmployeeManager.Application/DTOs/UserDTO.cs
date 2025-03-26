namespace EmployeeManager.Application.DTOs
{
	public class UserDTO
	{
		public string UserDocument { get; set; }

		public string Password { get; set; }

		public UserDTO() { }

		public UserDTO(string userDocument, string password)
		{
			UserDocument = userDocument;
			Password = password;
		}
	}
}
