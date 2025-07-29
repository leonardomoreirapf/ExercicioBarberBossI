
using System.Net;

namespace BarberBossI.Exception.ExceptionsBase;

public class InvalidLoginException : FaturamentoException
{
	public InvalidLoginException() : base(ResourceErrorMessage.LoginOuSenhaInvalido)
	{
	}

	public override int StatusCode => (int)HttpStatusCode.Unauthorized;

	public override List<string> GetErrors()
	{
		return [Message];
	}
}
