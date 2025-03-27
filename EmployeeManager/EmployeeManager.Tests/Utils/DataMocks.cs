using EmployeeManager.Application.DTOs;
using EmployeeManager.Domain.Enums;
using EmployeeManager.Domain.Models;

namespace EmployeeManager.Tests.Utils
{
	internal class DataMocks
	{
		internal Employee EmployeePresident = new("Teste 1",
				"Presidente",
				"presidente@company.com",
				"12345678902",
				null,
				null,
				ERoleType.President,
				new DateTime(1996, 09, 18),
				true);

		internal EmployeeDTO EmployeePresidentDTO;

		internal Employee EmployeeDirector = new("Teste 2",
				"Diretor",
				"diretor@company.com",
				"12345678903",
				null,
				null,
				ERoleType.Director,
				new DateTime(1996, 09, 18),
				true);

		public DataMocks()
		{
			EmployeePresident.PhoneNumbers = [new PhoneNumber("47991075595", EmployeePresident), new PhoneNumber("47991075596", EmployeePresident)];
			EmployeePresidentDTO = new(EmployeePresident);

			EmployeeDirector.Manager = EmployeePresident;
			EmployeeDirector.PhoneNumbers = [new PhoneNumber("47991075597", EmployeePresident)];
		}
	}
}
