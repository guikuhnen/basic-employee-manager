using EmployeeManager.Tests.Utils;

namespace EmployeeManager.Tests.Domain
{
	public class EmployeeTests
	{
		[Fact]
		public void TestEmployee_IsMinor_Returns_False()
		{
			//Arrange
			DataMocks.EmployeePresident.BirthDate = DateTime.Now.AddYears(-18);

			//Act
			var result = DataMocks.EmployeePresident.IsMinor();

			//Assert
			Assert.False(result);
		}

		[Fact]
		public void TestEmployee_IsMinor_Returns_True()
		{
			//Arrange
			DataMocks.EmployeePresident.BirthDate = DateTime.Now.AddYears(-17);

			//Act
			var result = DataMocks.EmployeePresident.IsMinor();

			//Assert
			Assert.True(result);
		}

		[Fact]
		public void TestEmployee_CanBeManager_Returns_True()
		{
			//Arrange
			//Act
			var result = DataMocks.EmployeePresident.CanBeManager(DataMocks.EmployeeDirector.Role);

			//Assert
			Assert.True(result);
		}

		[Fact]
		public void TestEmployee_CanBeManager_Returns_False()
		{
			//Arrange
			//Act
			var result = DataMocks.EmployeeDirector.CanBeManager(DataMocks.EmployeePresident.Role);

			//Assert
			Assert.False(result);
		}
	}
}
