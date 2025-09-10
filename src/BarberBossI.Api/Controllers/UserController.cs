using BarberBossI.Application.UseCases.User.ChangePassword;
using BarberBossI.Application.UseCases.User.Profile;
using BarberBossI.Application.UseCases.User.Register;
using BarberBossI.Application.UseCases.User.Update;
using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
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

	[HttpGet]
	[Authorize]
	[ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetProfile(
		[FromServices] IGetUserProfileUseCase useCase)
	{
		var response = await useCase.Execute();

		return Ok(response);
	}

	[HttpPut]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateProfile(
		[FromServices] IUpdateUserUseCase useCase,
		[FromBody] RequestUpdateUserJson request)
	{
		await useCase.Execute(request);

		return NoContent();
	}

	[HttpPut("change-password")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ChangeProfile(
		[FromServices] IChangePasswordUseCase useCase,
		[FromBody] RequestChangePasswordJson request)
	{
		await useCase.Execute(request);

		return NoContent();
	}
}
