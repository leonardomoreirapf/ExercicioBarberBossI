using BarberBossI.Application.UseCases.User.Register;
using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBossI.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register(
		[FromServices] IRegisterUserUseCase useCase,
		[FromBody] RequestRegisterUserJson request)
	{
		var response = await useCase.Execute(request);

		return Created(string.Empty, response);
	}
}
