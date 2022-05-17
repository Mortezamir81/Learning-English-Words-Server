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
			var result =
				await NotificationServices.GetAllNotificationsAsync();

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}


		[HttpGet("GetLastVersionOfWPF")]
		public async Task<IActionResult> GetLastVersionsOfWPFAsync()
		{
			var result =
				await NotificationServices.GetLastVersionOfWPFAsync();

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		#endregion /HttpGet

		#region HttpPost
		[Authorize(UserRoles.All)]
		[HttpPost("Ticket")]
		public async Task<IActionResult> AddTicketAsync
			(AddTicketRequestViewModel addTicketRequestViewModel)
		{
			var result =
				await NotificationServices.AddTicketAsync(addTicketRequestViewModel);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost]
		public async Task<IActionResult> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel)
		{
			var result =
				await NotificationServices.SendNotificationForAllUserAsync(sendNotificationForAllUserRequestViewModel);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("User")]
		public async Task<IActionResult> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel)
		{
			var result =
				await NotificationServices.SendNotificationForSpeceficUserAsync(sendNotificationForSpeceficUserRequestViewModel);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		#endregion /HttpPost

		#region HttpDelete
		[Authorize(UserRoles.All)]
		[HttpDelete("{notificationId}")]
		public async Task<IActionResult> RemoveNotificationAsync(Guid notificationId)
		{
			var result =
				await NotificationServices.RemoveNotificationAsync(notificationId: notificationId);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		#endregion /HttpDelete
	}
}
