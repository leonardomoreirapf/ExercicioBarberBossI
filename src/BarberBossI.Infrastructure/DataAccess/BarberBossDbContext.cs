using BarberBossI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBossI.Infrastructure.DataAccess;

internal class BarberBossDbContext : DbContext
{
	public BarberBossDbContext(DbContextOptions options) : base(options){}
	public DbSet<Faturamento> Faturamentos { get; set; }

}
