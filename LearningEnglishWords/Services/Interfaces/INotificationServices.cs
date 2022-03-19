using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Services
{
	public interface INotificationServices
	{
		Task<Dtat.Results.Result> RemoveNotificationAsync(Guid notificationId);

		Task<Dtat.Results.Result<ApplicationVersions>> GetLastVersionOfWPFAsync();

		Task<Dtat.Results.Result> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel);

		Task<Dtat.Results.Result> AddTicketAsync(AddTicketRequestViewModel addTicketRequestViewModel);

		Task<Dtat.Results.Result<List<GetAllNotificationResponseViewModel>>> GetAllNotificationsAsync();

		Task<Dtat.Results.Result> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel);
	}
}
