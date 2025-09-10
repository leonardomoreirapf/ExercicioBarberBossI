using BarberBossI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBossI.Infrastructure.DataAccess;

public class BarberBossDbContext : DbContext
{
	public BarberBossDbContext(DbContextOptions options) : base(options){}
	public DbSet<Faturamento> Faturamentos { get; set; }
	public DbSet<User> Usuarios { get; set; }

}
