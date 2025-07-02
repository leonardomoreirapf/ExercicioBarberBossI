using BarberBossI.Communication.Responses;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BarberBossI.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		if(context.Exception is FaturamentoException)
		{
			HandleProjectException(context);
			return;
		}

		ThrowUnkowError(context);
	}

	private void HandleProjectException(ExceptionContext context)
	{
		var faturamentoException = (FaturamentoException)context.Exception;
		var errorResponse = new ResponseErrorJson(faturamentoException.GetErrors());

		context.HttpContext.Response.StatusCode = faturamentoException.StatusCode;
		context.Result = new ObjectResult(errorResponse);
	}

	private void ThrowUnkowError(ExceptionContext context)
	{
		var errorResponse = new ResponseErrorJson(ResourceErrorMessage.ErroDesconhecido);
		context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
		context.Result = new ObjectResult(errorResponse);
	}
}
