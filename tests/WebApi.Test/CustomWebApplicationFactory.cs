using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Enums;
using BarberBossI.Domain.Security.Cryptography;
using BarberBossI.Domain.Security.Tokens;
using BarberBossI.Infrastructure.DataAccess;
using CommonTesteUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	public FaturamentoIdentityManeger FaturamentoTeamMember { get; private set; } = default!;
	public FaturamentoIdentityManeger FaturamentoAdmin { get; private set; } = default!;
	public UserIdentityManager UserTeamMember { get; private set; } = default!;
	public UserIdentityManager UserAdmin { get; private set; } = default!;

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Test")
			.ConfigureServices(services => 
			{
				var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

				services.AddDbContext<BarberBossDbContext>(config => 
				{
					config.UseInMemoryDatabase("InMemoryDbForTesting");
					config.UseInternalServiceProvider(provider);
				});

				var scope = services.BuildServiceProvider().CreateScope();
				var dbContext = scope.ServiceProvider.GetRequiredService<BarberBossDbContext>();
				var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
				var accesTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

				StartDataBase(dbContext, passwordEncripter, accesTokenGenerator);
			});
	}


	private void StartDataBase(BarberBossDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accesTokenGenerator)
	{
		var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, accesTokenGenerator);
		var faturamentoTeamMember = AddFaturamentos(dbContext, userTeamMember, faturamentoId: 1);
		FaturamentoTeamMember = new FaturamentoIdentityManeger(faturamentoTeamMember);

		var userAdmin = AddUserAdmin(dbContext, passwordEncripter, accesTokenGenerator);
		var faturamentoAdmin = AddFaturamentos(dbContext, userAdmin, faturamentoId: 2);
		FaturamentoAdmin = new FaturamentoIdentityManeger(faturamentoAdmin);

		dbContext.SaveChanges();
	}

	private Faturamento AddFaturamentos(BarberBossDbContext dbContext, User user, long faturamentoId)
	{
		var faturamento = FaturamentoBuilder.Build(user);
		faturamento.Id = faturamentoId;

		dbContext.Faturamentos.Add(faturamento);

		return faturamento;
	}

	private User AddUserTeamMember(BarberBossDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
	{
		var user = UserBuilder.Build();
		user.Id = 1;

		var password = user.Password;

		user.Password = passwordEncripter.Encrypt(user.Password);

		dbContext.Usuarios.Add(user);

		var token = accessTokenGenerator.Generate(user);

		UserTeamMember = new UserIdentityManager(user, password, token);

		return user;
	}

	private User AddUserAdmin(BarberBossDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
	{
		var user = UserBuilder.Build(Roles.Admin);
		user.Id = 2;

		var password = user.Password;

		user.Password = passwordEncripter.Encrypt(user.Password);

		dbContext.Usuarios.Add(user);

		var token = accessTokenGenerator.Generate(user);

		UserAdmin = new UserIdentityManager(user, password, token);

		return user;
	}
}
