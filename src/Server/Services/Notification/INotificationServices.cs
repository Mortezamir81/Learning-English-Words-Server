namespace Services
{
	public interface INotificationServices
	{
		Task<Result> RemoveNotificationAsync(Guid notificationId, Guid userId);


		Task<Result<ApplicationVersion>> GetLastVersionOfWPFAsync();


		Task<Result> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel);


		Task<Result> AddTicketAsync(AddTicketRequestViewModel addTicketRequestViewModel, Guid userId);


		Task<Result<List<GetAllNotificationResponseViewModel>>> GetAllNotificationsAsync(Guid userId);


		Task<Result> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel);
	}
}
