using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmployeeManager.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController(ILogger<AuthController> logger, ILoginService loginService) : ControllerBase
	{
		private readonly ILogger<AuthController> _logger = logger;
		private readonly ILoginService _loginService = loginService;

		[HttpPost]
		[Route("signin")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TokenDTO))]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public IActionResult SignIn([FromBody] UserDTO user)
		{
			if (user is null)
			{
				return BadRequest();
			}

			var token = _loginService.ValidateCredentials(user);

			if (token is null)
			{
				return Unauthorized();
			}

			return Ok(token);
		}

		[HttpPost]
		[Route("refresh")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TokenDTO))]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public IActionResult Refresh([FromBody] TokenDTO tokenDTO)
		{
			if (tokenDTO is null)
			{
				return BadRequest();
			}

			var token = _loginService.ValidateCredentials(tokenDTO);

			if (token is null)
			{
				return BadRequest();
			}

			return Ok(token);
		}

		[HttpGet]
		[Route("revoke")]
		[Authorize("Bearer")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public IActionResult Revoke()
		{
			var userName = User.Identity?.Name;

			if (string.IsNullOrWhiteSpace(userName))
			{
				return BadRequest();
			}

			var result = _loginService.RevokeToken(userName);

			if (!result)
			{
				return BadRequest();
			}

			return NoContent();
		}
	}
}