using EmployeeManager.API.Configuration;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Services;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using EmployeeManager.Tests.Utils;
using Moq;
using System.Security.Claims;

namespace EmployeeManager.Tests.Application
{
	public class LoginServiceTests
	{
		private readonly Mock<IUserRepository> _userRepositoryMock = new();
		private readonly Mock<ITokenService> _tokenServiceMock = new();
		private readonly Mock<IEmployeeService> _employeeServiceMock = new();
		private readonly TokenConfiguration _tokenConfigurationMock = new TokenConfiguration()
		{
			Audience = "Audience",
			Issuer = "Issuer",
			Secret = "MY_NEWEST_SUPER_SUPER_SECRET_KEYMY_NEWEST_SUPER_SUPER_SECRET_KEY",
			Minutes = 60,
			DaysToExpire = 7
		};

		private readonly LoginService _service;

		public LoginServiceTests()
		{
			_service = new LoginService(_tokenConfigurationMock, _userRepositoryMock.Object, _tokenServiceMock.Object, _employeeServiceMock.Object);
		}

		[Fact]
		public void Test_ValidateCredentials_Execution_Completes()
		{
			//Arrange
			var userDTO = new UserDTO("123456", "abc123");
			var user = new User("123456", "abc123", string.Empty);

			_userRepositoryMock.Setup(x => x.ValidateCredentials(It.IsAny<User>())).Returns(user);
			_employeeServiceMock.Setup(x => x.GetEmployeeByDocument(It.IsAny<string>())).Returns(Task.FromResult(new DataMocks().EmployeePresidentDTO));

			_tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns("AccessToken");
			_tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("RefreshToken");
			_userRepositoryMock.Setup(x => x.RefreshUserInfo(It.IsAny<User>())).Returns(user);

			//Act
			var result = _service.ValidateCredentials(userDTO);

			//Assert
			Assert.NotNull(result);
			Assert.NotNull(result.AccessToken);
			Assert.NotNull(result.RefreshToken);
		}

		[Fact]
		public void Test_ValidateCredentials_User_Is_Null_Returns_Null()
		{
			//Arrange
			var userDTO = new UserDTO("123456", "abc123");
			User user = null;

			_userRepositoryMock.Setup(x => x.ValidateCredentials(It.IsAny<User>())).Returns(user);

			//Act
			var result = _service.ValidateCredentials(userDTO);

			//Assert
			Assert.Null(result);
		}

		[Fact]
		public void Test_ValidateCredentials_Employee_Is_Null_Returns_Null()
		{
			//Arrange
			//Arrange
			var userDTO = new UserDTO("123456", "abc123");
			var user = new User("123456", "abc123", string.Empty);
			EmployeeDTO employee = null;

			_userRepositoryMock.Setup(x => x.ValidateCredentials(It.IsAny<User>())).Returns(user);
			_employeeServiceMock.Setup(x => x.GetEmployeeByDocument(It.IsAny<string>())).Returns(Task.FromResult(employee));

			//Act
			var result = _service.ValidateCredentials(userDTO);

			//Assert
			Assert.Null(result);
		}

		[Fact]
		public void Test_ValidateCredentials_Token_Execution_Completes()
		{
			//Arrange
			var tokenDTO = new TokenDTO()
			{
				AccessToken = "AccessToken",
				RefreshToken = "RefreshToken"
			};
			var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test")]));
			var user = new User("123456", "abc123", string.Empty)
			{
				RefreshToken = tokenDTO.RefreshToken,
				RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
			};

			_tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>())).Returns(claimsPrincipal);
			_userRepositoryMock.Setup(x => x.ValidateCredentials(It.IsAny<string>())).Returns(user);
			_tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>())).Returns(tokenDTO.AccessToken);
			_tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(tokenDTO.RefreshToken);

			//Act
			var result = _service.ValidateCredentials(tokenDTO);

			//Assert
			Assert.NotNull(result);
			Assert.NotNull(result.AccessToken);
			Assert.NotNull(result.RefreshToken);
		}

		[Fact]
		public void Test_ValidateCredentials_Token_User_Is_Null_Returns_Null()
		{
			//Arrange
			var tokenDTO = new TokenDTO()
			{
				AccessToken = "AccessToken",
				RefreshToken = "RefreshToken"
			};
			var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test")]));
			User user = null;

			_tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>())).Returns(claimsPrincipal);
			_userRepositoryMock.Setup(x => x.ValidateCredentials(It.IsAny<string>())).Returns(user);

			//Act
			var result = _service.ValidateCredentials(tokenDTO);

			//Assert
			Assert.Null(result);
		}

		[Fact]
		public void Test_RevokeToken_Returns_True()
		{
			//Arrange
			_userRepositoryMock.Setup(x => x.RevokeToken(It.IsAny<string>())).Returns(true);
			//Act
			var result = _service.RevokeToken("abc123");
			//Assert
			Assert.True(result);
		}
	}
}
