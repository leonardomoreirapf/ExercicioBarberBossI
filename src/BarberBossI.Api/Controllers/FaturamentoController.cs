using BarberBossI.Application.UseCases.Faturamento.Delete;
using BarberBossI.Application.UseCases.Faturamento.GetAll;
using BarberBossI.Application.UseCases.Faturamento.GetById;
using BarberBossI.Application.UseCases.Faturamento.Register;
using BarberBossI.Application.UseCases.Faturamento.Update;
using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBossI.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FaturamentoController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseRegisteredFaturamentoJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register([FromServices] IRegisterFaturamentoUseCase useCase, 
								  [FromBody] RequestFaturamentoJson request)
	{
		var response = await useCase.Execute(request);

		return Created(string.Empty, response);
	}

	[HttpGet]
	[ProducesResponseType(typeof(ResponseFaturamentosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> GetAllFaturamento([FromServices] IGetAllFaturamentosUseCase useCase)
	{
		var response = await useCase.Execute();

		if (response.Faturamentos.Count().Equals(0))
			return NoContent();

		return Ok(response);
	}

	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(typeof(ResponseFaturamentoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById([FromServices] IGetFaturamentoByIdUseCase useCase,
											 [FromRoute] long id)
	{
		var response = await useCase.Execute(id);

		return Ok(response);
	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete([FromServices] IDeleteFaturamentoUseCase useCase,
											[FromRoute] long id)
	{
		await useCase.Execute(id);

		return NoContent();
	}

	[HttpPut]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Update([FromServices] IUpdateFaturamentoUseCase useCase,
											[FromRoute] long id,
											[FromBody] RequestFaturamentoJson request)
	{
		await useCase.Execute(id, request);

		return NoContent();
	}
}
