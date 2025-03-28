using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmployeeManager.API.Controllers
{
	[ApiController]
	[Authorize("Bearer")]
	[Route("[controller]")]
    public class EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService) : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger = logger;
        private readonly IEmployeeService _employeeService = employeeService;

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Create([FromBody] EmployeeDTO employee)
		{
			try
			{
				if (employee is null)
				{
					return BadRequest();
				}

				await _employeeService.AddEmployee(employee, User.Identity?.Name);

				return NoContent();
			}
			catch (Exception e)
			{
				if (e.InnerException is not null)
				{
					return ReturnError("Create", e.InnerException.Message);
				}

				return ReturnError("Create", e.Message);
			}
		}

		[HttpGet]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<EmployeeDTO>))]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				return Ok(await _employeeService.GetAllEmployees());
			}
			catch (Exception e)
			{
				if (e.InnerException is not null)
				{
					return ReturnError("GetAll", e.InnerException.Message);
				}

				return ReturnError("GetAll", e.Message);
			}
		}

		[HttpGet("{id}")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EmployeeDTO))]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Get(long id)
		{
			try
			{
				var employee = await _employeeService.GetEmployee(id);

				if (employee is null)
				{
					return NotFound();
				}

				return Ok(employee);
			}
			catch (Exception e)
			{
				if (e.InnerException is not null)
				{
					return ReturnError("Get", e.InnerException.Message);
				}

				return ReturnError("Get", e.Message);
			}
		}

		[HttpPut]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Update([FromBody] EmployeeDTO employee)
		{
			try
			{
				if (employee is null)
				{
					return BadRequest();
				}

				await _employeeService.UpdateEmployee(employee);

				return NoContent();
			}
			catch (Exception e)
			{
				if (e.InnerException is not null)
				{
					return ReturnError("Update", e.InnerException.Message);
				}

				return ReturnError("Update", e.Message);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				await _employeeService.DeleteEmployee(id);

				return NoContent();
			}
			catch (Exception e)
			{
				if (e.InnerException is not null)
				{
					return ReturnError("Delete", e.InnerException.Message);
				}

				return ReturnError("Delete", e.Message);
			}
		}

		#region PRIVATE

		private ObjectResult ReturnError(string methodName, string errorMessage)
		{
			var message = $"EmployeeController - {methodName} - ERROR - {errorMessage}";

			_logger.LogError("{Message}", message);

			return StatusCode((int)HttpStatusCode.InternalServerError, new { message = errorMessage });
		}

		#endregion
	}
}
