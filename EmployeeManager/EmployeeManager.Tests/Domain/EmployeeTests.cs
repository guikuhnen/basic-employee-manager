using EmployeeManager.Tests.Utils;

namespace EmployeeManager.Tests.Domain
{
	public class EmployeeTests
	{
		[Fact]
		public void TestEmployee_IsMinor_Returns_False()
		{
			//Arrange
			var employee = new DataMocks().EmployeePresident;
			employee.BirthDate = DateTime.Now.AddYears(-18);

			//Act
			var result = employee.IsMinor();

			//Assert
			Assert.False(result);
		}

		[Fact]
		public void TestEmployee_IsMinor_Returns_True()
		{
			//Arrange
			var employee = new DataMocks().EmployeePresident;
			employee.BirthDate = DateTime.Now.AddYears(-17);

			//Act
			var result = employee.IsMinor();

			//Assert
			Assert.True(result);
		}

		[Fact]
		public void TestEmployee_CanBeManager_Returns_True()
		{
			//Arrange
			var employee = new DataMocks().EmployeePresident;
			var manager = new DataMocks().EmployeeDirector;

			//Act
			var result = employee.CanBeManager(manager.Role);

			//Assert
			Assert.True(result);
		}

		[Fact]
		public void TestEmployee_CanBeManager_Returns_False()
		{
			//Arrange
			var employee = new DataMocks().EmployeeDirector;
			var manager = new DataMocks().EmployeePresident;

			//Act
			var result = employee.CanBeManager(manager.Role);

			//Assert
			Assert.False(result);
		}
	}
}
