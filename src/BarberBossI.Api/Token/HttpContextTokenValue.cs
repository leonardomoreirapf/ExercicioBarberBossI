using BarberBossI.Domain.Security.Tokens;

namespace BarberBossI.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
	private readonly IHttpContextAccessor _contextAccessor;

	public HttpContextTokenValue(IHttpContextAccessor httpContext)
	{
		_contextAccessor = httpContext;
	}

	public string TokenOnRequest()
	{
		var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

		return authorization["Bearer ".Length..].Trim();
	}
}
