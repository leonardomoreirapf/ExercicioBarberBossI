using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace BarberBossI.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
	private readonly BarberBossDbContext _context;

	public UserRepository(BarberBossDbContext context) => _context = context;

	public async Task Add(User user)
	{
		await _context.Usuarios.AddAsync(user);
	}

	public async Task<bool> ExistActiveUserWithEmail(string email)
	{
		return await _context.Usuarios.AnyAsync(user => user.Email.Equals(email));
	}

	public async Task<User?> GetUserByEmail(string email)
	{
		return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
	}
}
