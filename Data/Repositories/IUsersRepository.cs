using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Persistence.Repositories
{
	public interface IUsersRepository : Softmax.Data.IRepository<Users>
	{
		Task UpdateUserAsync(Users users);

		Task<bool> CheckEmailExist(string email);

		Task<bool> CheckUsernameExist(string username);

		Task<Users> LoginAsync(string username, string password);

		Task<bool> CheckUserSecurityStampAsync(Guid securityStamp);
	}
}
