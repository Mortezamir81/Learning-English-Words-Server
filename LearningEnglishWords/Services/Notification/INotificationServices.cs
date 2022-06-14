namespace Services
{
	public interface INotificationServices
	{
		Task<Result> RemoveNotificationAsync(Guid notificationId);


		Task<Result<ApplicationVersion>> GetLastVersionOfWPFAsync();


		Task<Result> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel);


		Task<Result> AddTicketAsync(AddTicketRequestViewModel addTicketRequestViewModel);


		Task<Result<List<GetAllNotificationResponseViewModel>>> GetAllNotificationsAsync();


		Task<Result> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel);
	}
}
