using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Persistence.Repositories
{
	public interface IUsersRepository : Dtat.Data.IRepository<User>
	{
		Task UpdateUserAsync(User users);

		Task<bool> CheckEmailExist(string email);

		Task<bool> CheckUsernameExist(string username);

		Task<User> LoginAsync(string username, string password);

		Task<bool> CheckUserSecurityStampAsync(Guid securityStamp);
	}
}
