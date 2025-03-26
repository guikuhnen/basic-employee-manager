using EmployeeManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Data.Context
{
	internal class MySQLContext : DbContext
	{
		public MySQLContext() { }

		public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

		public DbSet<Employee> Employees { get; set; }
		public DbSet<PhoneNumber> PhoneNumbers { get; set; }
	}
}
