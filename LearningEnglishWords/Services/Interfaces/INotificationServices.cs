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
		Task<Softmax.Results.Result> RemoveNotificationAsync(Guid notificationId);

		Task<Softmax.Results.Result<ApplicationVersions>> GetLastVersionOfWPFAsync();

		Task<Softmax.Results.Result> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel);

		Task<Softmax.Results.Result> AddTicketAsync(AddTicketRequestViewModel addTicketRequestViewModel);

		Task<Softmax.Results.Result<List<GetAllNotificationResponseViewModel>>> GetAllNotificationsAsync();

		Task<Softmax.Results.Result> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel);
	}
}
