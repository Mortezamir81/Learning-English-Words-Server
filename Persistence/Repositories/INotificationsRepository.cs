using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public interface INotificationsRepository : Dtat.Data.IRepository<Notifications>
	{
		Task<List<Notifications>> GetAllNotification(Guid userId);

		Task<Notifications> GetNotification(Guid notificationId, Guid userId);
	}
}
