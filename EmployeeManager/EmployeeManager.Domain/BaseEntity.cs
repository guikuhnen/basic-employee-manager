using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Domain
{
	public abstract class BaseEntity
	{
		[Key]
		public long Id { get; set; }
	}
}
