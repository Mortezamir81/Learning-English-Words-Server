namespace Server.Controllers
{
	public class NotificationsController : BaseApiControllerWithDatabase
	{
		#region Constractor
		public NotificationsController
			(IUnitOfWork unitOfWork,
			Dtat.Logging.ILogger<NotificationsController> logger,
			INotificationServices notificationServices) : base(unitOfWork)
		{
			Logger = logger;
			NotificationServices = notificationServices;
		}
		#endregion /Constractor

		#region Properties
		public Dtat.Logging.ILogger<NotificationsController> Logger { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		public INotificationServices NotificationServices { get; }
		#endregion /Properties

		#region HttpGet
		[HttpGet("CheckServer")]
		public IActionResult CheckServer()
		{
			return Ok(true);
		}


		[Authorize(UserRoles.All)]
		[HttpGet]
		public async Task<IActionResult> GetAllNotification()
		{
			var serviceResult =
				await NotificationServices.GetAllNotificationsAsync();

			return serviceResult.ApiResult();
		}


		[HttpGet("GetLastVersionOfWPF")]
		public async Task<IActionResult> GetLastVersionsOfWPFAsync()
		{
			var serviceResult =
				await NotificationServices.GetLastVersionOfWPFAsync();

			return serviceResult.ApiResult();
		}
		#endregion /HttpGet

		#region HttpPost
		[Authorize(UserRoles.All)]
		[HttpPost("Ticket")]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult> AddTicketAsync
			(AddTicketRequestViewModel requestViewModel)
		{
			var serviceResult =
				await NotificationServices.AddTicketAsync(requestViewModel);

			return serviceResult.ApiResult();
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel requestViewModel)
		{
			var serviceResult =
				await NotificationServices.SendNotificationForAllUserAsync(requestViewModel);

			return serviceResult.ApiResult();
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("User")]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel requestViewModel)
		{
			var serviceResult =
				await NotificationServices.SendNotificationForSpeceficUserAsync(requestViewModel);

			return serviceResult.ApiResult();
		}
		#endregion /HttpPost

		#region HttpDelete
		[Authorize(UserRoles.All)]
		[HttpDelete("{notificationId}")]
		public async Task<IActionResult> RemoveNotificationAsync(Guid notificationId)
		{
			var serviceResult =
				await NotificationServices.RemoveNotificationAsync(notificationId: notificationId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpDelete
	}
}
