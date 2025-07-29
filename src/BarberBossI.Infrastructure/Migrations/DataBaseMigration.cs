using BarberBossI.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBossI.Infrastructure.Migrations;

public static class DataBaseMigration
{
	public async static Task MigrateDataBase(IServiceProvider serviceProvider)
	{
		var dbContext = serviceProvider.GetRequiredService<BarberBossDbContext>();

		await dbContext.Database.MigrateAsync();
	}
}
