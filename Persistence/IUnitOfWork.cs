using Persistence.Repositories;
using System.Threading.Tasks;

namespace Persistence
{
	public interface IUnitOfWork : Softmax.Data.IUnitOfWork
	{
		IUsersRepository UserRepository { get; }
		IWordsRepository WordsRepository { get; }
		INotificationsRepository NotificationsRepository { get; }
		Task ClearTracking();
	}
}
