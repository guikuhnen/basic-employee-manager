using EmployeeManager.Data.Context;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Domain.Models;
using EmployeeManager.Tests.Utils;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Tests.Data
{
	public class UserRepositoryTests
	{
		private readonly UserRepository _repository;
		private readonly DbContextOptions<MySQLContext> _options;
		private readonly MySQLContext _context;
		private readonly List<User> _users;

		public UserRepositoryTests()
		{
			_options = new DbContextOptionsBuilder<MySQLContext>()
				.UseInMemoryDatabase(databaseName: "UserDatabase")
				.Options;

			_users =
			[
				new()
				{
					Id = 1,
					FullName = "admin",
					Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
					UserDocument = "12345678900",
					RefreshToken = "RefreshToken"
				},
				new()
				{
					Id = 2,
					FullName = "user",
					Password = "04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb",
					UserDocument = "98765432100"
				}
			];

			_context = new MySQLContext(_options);
			if (_context.Users.Count() < 2)
			{
				_context.Users.AddRange(_users);
				_context.SaveChanges();
			}

			_repository = new UserRepository(_context);
		}

		[Fact]
		public void Test_ValidateCredentials_Execution_Completes()
		{
			//Arrange
			var user = new User()
			{
				Id = 1,
				FullName = "admin",
				Password = "admin",
				UserDocument = "12345678900"
			};

			//Act
			var result = _repository.ValidateCredentials(user);

			//Assert
			Assert.NotNull(result);
			Assert.Equal(_users[0].Id, user.Id);
		}

		[Fact]
		public void Test_ValidateCredentials_Document_Execution_Completes()
		{
			//Arrange
			var user = new User()
			{
				Id = 2,
				FullName = "user",
				Password = "user",
				UserDocument = "98765432100"
			};

			//Act
			var result = _repository.ValidateCredentials(user.UserDocument);

			//Assert
			Assert.NotNull(result);
			Assert.Equal(_users[1].Id, user.Id);
		}

		[Fact]
		public void Test_RefreshUserInfo_Execution_Completes()
		{
			//Arrange
			var user = new User()
			{
				Id = 1,
				FullName = "admin1",
				Password = "admin2",
				UserDocument = "12345678900"
			};

			//Act
			var result = _repository.RefreshUserInfo(user);

			//Assert
			Assert.NotNull(result);
			Assert.Equal(user.FullName, result.FullName);
		}

		[Fact]
		public void Test_RefreshUserInfo_Returns_Null()
		{
			//Arrange
			var user = new User()
			{
				Id = 3,
				FullName = "admin3",
				Password = "admin3",
				UserDocument = "12345678903"
			};

			//Act
			var result = _repository.RefreshUserInfo(user);

			//Assert
			Assert.Null(result);
		}

		[Fact]
		public void Test_RevokeToken_Execution_Completes()
		{
			//Arrange
			var userDocument = "12345678900";

			//Act
			var result = _repository.RevokeToken(userDocument);

			//Assert
			Assert.True(result);
			Assert.Equal(null, _context.Users.Where(x => x.UserDocument.Equals(userDocument)).FirstOrDefault().RefreshToken);
		}

		[Fact]
		public void Test_RevokeToken_Returns_False()
		{
			//Arrange
			var userDocument = "12345678903";

			//Act
			var result = _repository.RevokeToken(userDocument);

			//Assert
			Assert.False(result);
		}

		[Fact]
		public void Test_AddUser_Execution_Completes()
		{
			//Arrange
			var user = new User()
			{
				Id = 3,
				FullName = "admin3",
				Password = "admin3",
				UserDocument = "12345678903"
			};

			//Act
			var result = _repository.AddUser(user);

			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
			Assert.Equal(3, _context.Users.Count());
		}

		[Fact]
		public void Test_UpdatePassword_Execution_Completes()
		{
			//Arrange
			var documentNumber = "98765432100";
			var newPassword = "admin4";

			//Act
			var result = _repository.UpdatePassword(documentNumber, newPassword);

			//Assert
			Assert.NotNull(result);
			Assert.True(result.IsCompletedSuccessfully);
		}
	}
}
