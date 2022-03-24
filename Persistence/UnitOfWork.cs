using Persistence.Repositories;
using System.Threading.Tasks;

namespace Persistence
{
	public class UnitOfWork :
		Dtat.Data.EntityFrameworkCore.UnitOfWork<DatabaseContext>, IUnitOfWork
	{
		public UnitOfWork(DatabaseContext databaseContext) : base(databaseContext)
		{
		}


		#region Fields And Properties
		//********
		private IUsersRepository _userRepository;

		public IUsersRepository UserRepository
		{
			get
			{
				if (_userRepository == null)
				{
					_userRepository =
						new UsersRepository(databaseContext: DatabaseContext);
				}

				return _userRepository;
			}
		}
		//********

		//********
		private IWordsRepository _wordsRepository;

		public IWordsRepository WordsRepository
		{
			get
			{
				if (_wordsRepository == null)
				{
					_wordsRepository =
						new WordsRepository(databaseContext: DatabaseContext);
				}

				return _wordsRepository;
			}
		}
		//********

		//********
		private INotificationsRepository _notificationsRepository;

		public INotificationsRepository NotificationsRepository
		{
			get
			{
				if (_notificationsRepository == null)
				{
					_notificationsRepository =
						new NotificationsRepository(databaseContext: DatabaseContext);
				}

				return _notificationsRepository;
			}
		}
		//********
		#endregion /Fields And Properties

		public async override Task<int> SaveAsync()
		{
			var rowsAffected =
				await DatabaseContext.SaveChangesAsync();

			DatabaseContext.ChangeTracker.Clear();

			return rowsAffected;
		}

		public async Task ClearTracking()
		{
			await Task.Run(() =>
			{
				DatabaseContext.ChangeTracker.Clear();
			});
		}
	}
}
