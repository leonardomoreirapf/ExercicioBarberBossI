using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Security.Tokens;
using BarberBossI.Domain.Services.LoggedUser;
using BarberBossI.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BarberBossI.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
	private readonly BarberBossDbContext _dbContext;
	private readonly ITokenProvider _tokenProvider;

	public LoggedUser(BarberBossDbContext dbContext, ITokenProvider tokenProvider)
	{
		_dbContext = dbContext;
		_tokenProvider = tokenProvider;
	}

	public async Task<User> Get()
	{
		var token = _tokenProvider.TokenOnRequest();

		var tokenHandler = new JwtSecurityTokenHandler();

		var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

		var identifier = jwtSecurityToken.Claims.First(claim => claim.Type.Equals(ClaimTypes.Sid)).Value;

		return await _dbContext.Usuarios.AsNoTracking().FirstAsync(user => user.UserIndentifier.Equals(Guid.Parse(identifier)));
	}
}
