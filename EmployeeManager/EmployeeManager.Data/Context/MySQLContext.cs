using EmployeeManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Data.Context
{
	public class MySQLContext : DbContext
	{
		public MySQLContext() { }

		public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

		public DbSet<Employee> Employees { get; set; }
		public DbSet<PhoneNumber> PhoneNumbers { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Employee>()
				.HasIndex(u => u.DocumentNumber)
				.IsUnique();

			modelBuilder.Entity<User>()
				.HasIndex(u => u.UserDocument)
				.IsUnique();
		}
	}
}
