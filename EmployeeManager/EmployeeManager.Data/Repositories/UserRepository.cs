using EmployeeManager.Data.Context;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
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

		#region LOGIN

		public User? ValidateCredentials(User user)
		{
			var hashPass = ComputeHash(user.Password, SHA256.Create());

			return _context.Users.FirstOrDefault(u => (u.UserDocument == user.UserDocument) && (u.Password == hashPass));
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

		#endregion

		#region CRUD

		/// <summary>
		/// Adds a new user to the database
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task AddUser(User user)
		{
			var hashPass = ComputeHash(user.Password, SHA256.Create());

			user.Password = hashPass;

			_context.Add(user);

			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Updates the user password in the database
		/// </summary>
		/// <param name="documentNumber"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task UpdatePassword(string documentNumber, string password)
		{
			var user = await _context.Users.Where(u => u.UserDocument.Equals(documentNumber)).FirstOrDefaultAsync();

			if (user is not null)
			{
				var hashPass = ComputeHash(password, SHA256.Create());

				user.Password = hashPass;

				_context.Update(user);

				await _context.SaveChangesAsync();
			}
		}

		#endregion

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
