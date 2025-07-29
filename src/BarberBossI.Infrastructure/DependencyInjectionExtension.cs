using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Domain.Repositories.User;
using BarberBossI.Domain.Security.Cryptography;
using BarberBossI.Domain.Security.Tokens;
using BarberBossI.Infrastructure.DataAccess;
using BarberBossI.Infrastructure.DataAccess.Repositories;
using BarberBossI.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBossI.Infrastructure;

public static class DependencyInjectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
	{
		AddDbContext(services, configuration);
		AddToken(services, configuration);
		AddRepositories(services);

		services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
		
	}

	private static void AddToken(IServiceCollection services, IConfiguration configuration)
	{
		var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
		var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");

		services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signinKey!));
	}

	private static void AddRepositories(IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IFaturamentoReadOnlyRepository, FaturamentoRepository>();
		services.AddScoped<IFaturamentoWriteOnlyRepository, FaturamentoRepository>();
		services.AddScoped<IFaturamentoUpdateOnlyRepository, FaturamentoRepository>();
		services.AddScoped<IUserReadOnlyRepository, UserRepository>();
		services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
	}

	private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Connection");
		var serverVersion = new MySqlServerVersion(new Version(8, 0, 26));

		services.AddDbContext<BarberBossDbContext>(config => config.UseMySql(connectionString: connectionString, serverVersion: serverVersion));
	}
}
