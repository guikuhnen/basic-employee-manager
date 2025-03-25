using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManager.Domain
{
	internal abstract class BaseEntity
	{
		[Key]
		public long Id { get; set; }
	}
}
