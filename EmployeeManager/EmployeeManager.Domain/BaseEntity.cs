using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManager.Domain
{
	public abstract class BaseEntity
	{
		[Key]
		public long Id { get; set; }
	}
}
