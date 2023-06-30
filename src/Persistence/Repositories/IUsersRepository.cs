using Domain.Entities;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public interface IUsersRepository : Dtat.Data.IRepository<User>
	{
		Task UpdateUserAsync(User users);

		Task<bool> CheckEmailExist(string email);

		Task<bool> CheckUserNameExist(string userName);

		Task<User> LoginAsync(string userName, string password);

		Task<bool> CheckUserSecurityStampAsync(string securityStamp);
	}
}
