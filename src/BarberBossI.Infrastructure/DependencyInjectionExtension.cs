﻿using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Infrastructure.DataAccess;
using BarberBossI.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBossI.Infrastructure;

public static class DependencyInjectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
	{
		AddDbContext(services, configuration);
		AddRepositories(services);
	}

	private static void AddRepositories(IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IFaturamentoReadOnlyRepository, FaturamentoRepository>();
		services.AddScoped<IFaturamentoWriteOnlyRepository, FaturamentoRepository>();
		services.AddScoped<IFaturamentoUpdateOnlyRepository, FaturamentoRepository>();
	}

	private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Connection");
		var serverVersion = new MySqlServerVersion(new Version(8, 0, 26));

		services.AddDbContext<BarberBossDbContext>(config => config.UseMySql(connectionString: connectionString, serverVersion: serverVersion));
	}
}
