using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Services
{
	public abstract class BaseService(ILogger<BaseService> logger)
	{
		protected readonly ILogger<BaseService> _logger = logger;
	}
}
