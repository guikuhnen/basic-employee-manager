using EmployeeManager.Data.Context;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Data.Repositories
{
	public class UserRepository(MySQLContext context) : IUserRepository
	{
		private readonly MySQLContext _context = context;

		public User? ValidateCredentials(User user)
		{
			var pass = ComputeHash(user.Password, SHA256.Create());

			return _context.Users.FirstOrDefault(u => (u.UserDocument == user.UserDocument) && (u.Password == pass));
		}

		public User? ValidateCredentials(string userDocument)
		{
			return _context.Users.SingleOrDefault(u => u.UserDocument == userDocument);
		}

		public User? RefreshUserInfo(User user)
		{
			var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
			if (result != null)
				try
				{
					_context.Users.Entry(result).CurrentValues.SetValues(user);
					_context.SaveChanges();

					return result;
				}
				catch (Exception)
				{
					throw;
				}

			return null;
		}

		public bool RevokeToken(string userDocument)
		{
			var user = _context.Users.SingleOrDefault(u => u.UserDocument == userDocument);

			if (user == null)
				return false;

			user.RefreshToken = null;

			_context.SaveChanges();

			return true;
		}

		#region PRIVATE

		private static string ComputeHash(string input, HashAlgorithm hashAlgorithm)
		{
			var hashedBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

			var sBuilder = new StringBuilder();

			foreach (var item in hashedBytes)
				sBuilder.Append(item.ToString("x2"));

			return sBuilder.ToString();
		}

		#endregion
	}
}
