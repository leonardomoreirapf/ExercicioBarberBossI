using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace BarberBossI.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
	private readonly BarberBossDbContext _context;

	public UserRepository(BarberBossDbContext context) => _context = context;

	public async Task Add(User user)
	{
		await _context.Usuarios.AddAsync(user);
	}

	public async Task Delete(User user)
	{
		var userToRemove = await _context.Usuarios.FindAsync(user.Id);
		_context.Usuarios.Remove(userToRemove!);
	}

	public async Task<bool> ExistActiveUserWithEmail(string email)
	{
		return await _context.Usuarios.AnyAsync(user => user.Email.Equals(email));
	}

	public async Task<User> GetById(long id)
	{
		return await _context.Usuarios.FirstAsync(user => user.Id.Equals(id));
	}

	public async Task<User?> GetUserByEmail(string email)
	{
		return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
	}

	public void Update(User user)
	{
		_context.Usuarios.Update(user);
	}
}
