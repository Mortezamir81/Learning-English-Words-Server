namespace Server.Controllers.V1
{
	public class NotificationsController : BaseApiControllerWithDatabase
	{
		#region Constractor
		public NotificationsController
			(IUnitOfWork unitOfWork,
			ILogger<NotificationsController> logger,
			INotificationServices notificationServices) : base(unitOfWork)
		{
			Logger = logger;
			NotificationServices = notificationServices;
		}
		#endregion /Constractor

		#region Properties
		public ILogger<NotificationsController> Logger { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		public INotificationServices NotificationServices { get; }
		#endregion /Properties

		#region HttpGet
		[HttpGet("CheckServer")]
		public IActionResult CheckServer()
		{
			return Ok(true);
		}


		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetAllNotification()
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await NotificationServices.GetAllNotificationsAsync(userId: userId);

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
		[Authorize]
		[HttpPost("Ticket")]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult> AddTicketAsync
			(AddTicketRequestViewModel requestViewModel)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await NotificationServices.AddTicketAsync(requestViewModel, userId: userId);

			return serviceResult.ApiResult();
		}


		[Authorize(Roles = $"{Constants.Role.SystemAdmin}, {Constants.Role.Admin}")]
		[HttpPost]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel requestViewModel)
		{
			var serviceResult =
				await NotificationServices.SendNotificationForAllUserAsync(requestViewModel);

			return serviceResult.ApiResult();
		}


		[Authorize(Roles = $"{Constants.Role.SystemAdmin}, {Constants.Role.Admin}")]
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
		[Authorize]
		[HttpDelete("{notificationId}")]
		public async Task<IActionResult> RemoveNotificationAsync(Guid notificationId)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await NotificationServices.RemoveNotificationAsync(notificationId: notificationId, userId: userId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpDelete
	}
}
