using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public class UsersRepository :
		Dtat.Data.EntityFrameworkCore.Repository<User>, IUsersRepository
	{
		public UsersRepository(DatabaseContext databaseContext) : base(databaseContext: databaseContext)
		{
		}


		public async Task UpdateUserAsync(User users)
		{
			if (users == null)
				throw new ArgumentNullException(paramName: nameof(users));

			await Task.Run(() =>
			{
				DatabaseContext.Entry(users).State = EntityState.Modified;
				DatabaseContext.Entry(users).Property(x => x.TimeRegistered).IsModified = false;
				DatabaseContext.Entry(users).Property(x => x.Password).IsModified = false;
			});
		}


		public async Task<bool> CheckEmailExist(string email)

		{
			var result =
				await DbSet
					.AsNoTracking()
					.Select(current => current.Email)
					.Where(current => current == email)
					.AnyAsync()
					;

			return result;
		}


		public async Task<bool> CheckUserNameExist(string userName)
		{
			var result =
				await DbSet
					.AsNoTracking()
					.Select(current => current.UserName)
					.Where(current => current == userName)
					.AnyAsync()
					;

			return result;
		}


		public async Task<User> LoginAsync(string userName, string password)
		{
			if (string.IsNullOrWhiteSpace(userName))
			{
				return null;
			}

			if (string.IsNullOrWhiteSpace(password))
			{
				return null;
			}

			var result =
				await DbSet
					.AsNoTracking()
					.Where(current => current.UserName.ToLower() == userName.ToLower())
					.Where(current => current.Password == password)
					.Include(current => current.UserLogins)
					.SingleOrDefaultAsync()
					;

			return result;
		}


		public async Task<bool> CheckUserSecurityStampAsync(string securityStamp)
		{
			var result =
				await DbSet
					.AsNoTracking()
					.Select(current => current.SecurityStamp)
					.Where(current => current == securityStamp)
					.AnyAsync()
					;

			return result;
		}
	}
}
