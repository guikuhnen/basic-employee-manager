using EmployeeManager.Application.Services;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using EmployeeManager.Tests.Utils;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManager.Tests.Application
{
	public class EmployeeServiceTests
	{
		private readonly Mock<ILogger<EmployeeService>> _loggerMock = new();
		private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
		private readonly Mock<IPhoneNumberRepository> _phoneNumberRepositoryMock = new();
		private readonly Mock<IUserRepository> _userRepositoryMock = new();

		private readonly EmployeeService _service;

		public EmployeeServiceTests()
		{
			_service = new EmployeeService(_loggerMock.Object, _employeeRepositoryMock.Object, _phoneNumberRepositoryMock.Object, _userRepositoryMock.Object);
		}

		[Fact]
		public void Test_AddEmployee_Execution_Completes()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.AddEmployee(It.IsAny<Employee>())).Returns(Task.CompletedTask);
			_phoneNumberRepositoryMock.Setup(x => x.AddPhoneNumbers(It.IsAny<List<PhoneNumber>>())).Returns(Task.CompletedTask);
			_userRepositoryMock.Setup(x => x.AddUser(It.IsAny<User>())).Returns(Task.CompletedTask);
			var employee = new DataMocks().EmployeePresidentDTO;

			//Act
			var result = _service.AddEmployee(employee);

			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_AddEmployee_Throws_Exception()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.AddEmployee(It.IsAny<Employee>())).Throws(It.IsAny<Exception>());

			//Act
			var result = _service.AddEmployee(new DataMocks().EmployeePresidentDTO);

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_AddEmployee_Invalid_Manager_Throws_Exception()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.GetEmployee(It.IsAny<long>())).ReturnsAsync(new DataMocks().EmployeeDirector);
			var employeePresidentDTO = new DataMocks().EmployeePresidentDTO;
			employeePresidentDTO.ManagerId = new DataMocks().EmployeeDirector.Id;

			//Act
			var result = _service.AddEmployee(employeePresidentDTO);

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_AddEmployee_IsMinor_Throws_Exception()
		{
			//Arrange
			var employeePresidentDTO = new DataMocks().EmployeePresidentDTO;
			employeePresidentDTO.BirthDate = new DateTime(2020, 09, 18);

			//Act
			var result = _service.AddEmployee(employeePresidentDTO);

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}

		[Fact]
		public async void Test_GetAllEmployees_Returns_Two_Itens()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.GetAllEmployees()).ReturnsAsync([new DataMocks().EmployeePresident, new DataMocks().EmployeeDirector]);

			//Act
			var result = await _service.GetAllEmployees();

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public void Test_GetAllEmployees_Throws_Exception()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.GetAllEmployees()).Throws(It.IsAny<Exception>());

			//Act
			var result = _service.GetAllEmployees();

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_GetAllEmployees_Returns_Null()
		{
			//Arrange
			IEnumerable<Employee>? list = null;
			_employeeRepositoryMock.Setup(x => x.GetAllEmployees()).ReturnsAsync(list);

			//Act
			var result = _service.GetAllEmployees();

			//Assert
			Assert.Null(result.Result);
			Assert.True(result.IsCompletedSuccessfully);
		}

		[Fact]
		public async void Test_GetEmployee_Returns_Employee()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.GetEmployee(It.IsAny<long>())).ReturnsAsync(new DataMocks().EmployeePresident);

			//Act
			var result = await _service.GetEmployee(It.IsAny<long>());

			//Assert
			Assert.NotNull(result);
			Assert.Equal(new DataMocks().EmployeePresidentDTO.Id, result.Id);
		}

		[Fact]
		public void Test_GetEmployee_Throws_Exception()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.GetEmployee(It.IsAny<long>())).Throws(It.IsAny<Exception>());

			//Act
			var result = _service.GetEmployee(It.IsAny<long>());

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_UpdateEmployee_Execution_Completes()
		{
			//Arrange
			_phoneNumberRepositoryMock.Setup(x => x.DeletePhoneNumbersByEmployeeId(It.IsAny<long>())).Returns(Task.CompletedTask);
			_employeeRepositoryMock.Setup(x => x.UpdateEmployee(It.IsAny<Employee>())).Returns(Task.CompletedTask);
			_userRepositoryMock.Setup(x => x.UpdatePassword(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

			//Act
			var result = _service.UpdateEmployee(new DataMocks().EmployeePresidentDTO);

			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_UpdateEmployee_Throws_Exception()
		{
			//Arrange
			_phoneNumberRepositoryMock.Setup(x => x.DeletePhoneNumbersByEmployeeId(It.IsAny<long>())).Throws(It.IsAny<Exception>());

			//Act
			var result = _service.UpdateEmployee(new DataMocks().EmployeePresidentDTO);

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_DeleteEmployee_Execution_Completes()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.DeleteEmployee(It.IsAny<long>())).Returns(Task.CompletedTask);

			//Act
			var result = _service.DeleteEmployee(It.IsAny<long>());

			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
		}

		[Fact]
		public void Test_DeleteEmployee_Throws_Exception()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.DeleteEmployee(It.IsAny<long>())).Throws(It.IsAny<Exception>());

			//Act
			var result = _service.DeleteEmployee(It.IsAny<long>());

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}

		[Fact]
		public async void Test_GetEmployeeByDocument_Returns_EmployeeDTO()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.GetEmployeeByDocument(It.IsAny<string>())).ReturnsAsync(new DataMocks().EmployeePresident);

			//Act
			var result = await _service.GetEmployeeByDocument(It.IsAny<string>());

			//Assert
			Assert.NotNull(result);
			Assert.Equal(new DataMocks().EmployeePresidentDTO.Id, result.Id);
		}

		[Fact]
		public void Test_GetEmployeeByDocument_Throws_Exception()
		{
			//Arrange
			_employeeRepositoryMock.Setup(x => x.GetEmployeeByDocument(It.IsAny<string>())).Throws(It.IsAny<Exception>());

			//Act
			var result = _service.GetEmployeeByDocument(It.IsAny<string>());

			//Assert
			Assert.NotNull(result.Exception);
			Assert.False(result.IsCompletedSuccessfully);
		}
	}
}