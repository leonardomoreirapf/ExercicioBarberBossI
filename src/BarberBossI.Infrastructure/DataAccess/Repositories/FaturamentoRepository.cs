using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.Faturamentos;
using Microsoft.EntityFrameworkCore;

namespace BarberBossI.Infrastructure.DataAccess.Repositories;

internal class FaturamentoRepository : IFaturamentoReadOnlyRepository, IFaturamentoWriteOnlyRepository, IFaturamentoUpdateOnlyRepository
{
	private readonly BarberBossDbContext _dbContext;
	public FaturamentoRepository(BarberBossDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public async Task Add(Faturamento faturamento)
	{
		await _dbContext.AddAsync(faturamento);
	}


	public async Task<List<Faturamento>> GetAll(User user) => await _dbContext.Faturamentos.AsNoTracking().Where(faturamento => faturamento.UserId.Equals(user.Id)).ToListAsync();
	async Task<Faturamento?> IFaturamentoReadOnlyRepository.GetById(User user, long id) => await _dbContext.Faturamentos.AsNoTracking().FirstOrDefaultAsync(faturamento => faturamento.Id.Equals(id) && faturamento.UserId.Equals(user.Id));
	async Task<Faturamento?> IFaturamentoUpdateOnlyRepository.GetById(long userId, long id) => await _dbContext.Faturamentos.FirstOrDefaultAsync(faturamento => faturamento.Id.Equals(id) && faturamento.UserId.Equals(userId));
	public async Task Delete(long id)
	{
		var result = await _dbContext.Faturamentos.FindAsync(id);


		_dbContext.Faturamentos.Remove(result!);
	}

	public void Update(Faturamento faturamento)
	{
		_dbContext.Faturamentos.Update(faturamento);
	}

	public async Task<List<Faturamento>> FilterByMonth(User user, DateOnly date)
	{
		var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

		var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
		var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);
		
		return await _dbContext.
					  Faturamentos.
					  AsNoTracking().
					  Where(faturamento => faturamento.UserId.Equals(user.Id) && faturamento.Data >= startDate && faturamento.Data <= endDate).
					  OrderBy(faturamento => faturamento.Data).
					  ThenBy(faturamento => faturamento.Titulo).
					  ToListAsync();
	}
}
