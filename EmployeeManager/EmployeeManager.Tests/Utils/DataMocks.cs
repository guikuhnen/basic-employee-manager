using EmployeeManager.Application.DTOs;
using EmployeeManager.Domain.Enums;
using EmployeeManager.Domain.Models;

namespace EmployeeManager.Tests.Utils
{
	internal static class DataMocks
	{
		internal readonly static Employee EmployeePresident = new("Teste 1",
				"Presidente",
				"presidente@company.com",
				"12345678902",
				[new PhoneNumber("47991075595", EmployeePresident)],
				null,
				ERoleType.President,
				new DateTime(1996, 09, 18),
				true);

		internal readonly static EmployeeDTO EmployeePresidentDTO = new(EmployeePresident);

		internal readonly static Employee EmployeeDirector = new("Teste 2",
				"Diretor",
				"diretor@company.com",
				"12345678903",
				null,
				EmployeePresident,
				ERoleType.Director,
				new DateTime(1996, 09, 18),
				true);
	}
}
