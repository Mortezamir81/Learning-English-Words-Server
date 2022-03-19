using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public class NotificationsRepository :
		Softmax.Data.EntityFrameworkCore.Repository<Notifications>, INotificationsRepository
	{
		public NotificationsRepository(DatabaseContext databaseContext) : base(databaseContext)
		{
		
		}


		//******************** Get Sections ********************
		public async Task<List<Notifications>> GetAllNotification(Guid userId)
		{
			var notifications =
				await DbSet
				.AsNoTracking()
				.Where(current => current.UserId == userId)
				.Where(current => current.IsDeleted == false)
				.OrderBy(current => current.IsRead)
				.ThenBy(current => current.SentDate)
				.ToListAsync()
				;

			return notifications;
		}


		public async Task<Notifications> GetNotification(Guid notificationId, Guid userId)
		{
			var notifications =
				await DbSet
				.Where(current => current.UserId == userId)
				.Where(current => current.IsDeleted == false)
				.Where(current => current.Id == notificationId)
				.FirstOrDefaultAsync()
				;

			return notifications;
		}
		//****************************************
	}
}
