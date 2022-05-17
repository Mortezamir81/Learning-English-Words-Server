namespace Services
{
	public partial class NotificationServices : INotificationServices
	{
		#region Constractor
		public NotificationServices
			(IMapper mapper,
			IUnitOfWork unitOfWork,
			DatabaseContext databaseContext,
			IHubContext<SignalHub> hubContext,
			IHttpContextAccessor httpContextAccessor,
			Dtat.Logging.ILogger<NotificationServices> logger) : base()
		{
			Logger = logger;
			Mapper = mapper;
			HubContext = hubContext;
			UnitOfWork = unitOfWork;
			DatabaseContext = databaseContext;
			HttpContextAccessor = httpContextAccessor;
		}
		#endregion /Constractor

		#region Properties
		public IMapper Mapper { get; }
		public IUnitOfWork UnitOfWork { get; }
		public DatabaseContext DatabaseContext { get; }
		public IHubContext<SignalHub> HubContext { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		public Dtat.Logging.ILogger<NotificationServices> Logger { get; }
		#endregion /Properties

		#region Methods
		public async Task<Result<List
			<GetAllNotificationResponseViewModel>>> GetAllNotificationsAsync()
		{
			try
			{
				var result =
					new Result<List<GetAllNotificationResponseViewModel>>();

				var user =
					HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken;

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var notifications =
					await UnitOfWork.NotificationsRepository.GetAllNotification(userId: user.Id);

				if (notifications == null || notifications.Count == 0)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.NotificationsListEmpty);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				result.Value = new List<GetAllNotificationResponseViewModel>();

				foreach (var notification in notifications)
				{
					result.Value.Add(Mapper.Map<GetAllNotificationResponseViewModel>(notification));
				}

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoadNotificationsSuccessfull);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				await Logger.LogCritical(exception: ex, message: ex.Message);

				var result =
					new Result<List<GetAllNotificationResponseViewModel>>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Result> RemoveNotificationAsync(Guid notificationId)
		{
			try
			{
				var result = new Result();

				var user =
					HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken;

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var response =
					await UnitOfWork.NotificationsRepository.GetNotification(notificationId: notificationId, userId: user.Id);

				if (response == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.NotificationNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				response.IsDeleted = true;

				await UnitOfWork.SaveAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.DeleteNotificationSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				await Logger.LogCritical(exception: ex, message: ex.Message);

				var result = new Result();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Result<ApplicationVersion>> GetLastVersionOfWPFAsync()
		{
			try
			{
				var result =
					new Result<ApplicationVersion>();

				var response =
				await DatabaseContext.ApplicationVersions
					.AsNoTracking()
					.OrderByDescending(current => current.PublishDate)
					.FirstOrDefaultAsync()
					;

				result.Value = response;

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoadContentSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				await Logger.LogCritical(exception: ex, message: ex.Message);

				var result =
					new Result<ApplicationVersion>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Result> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel)
		{
			try
			{
				var result =
					 SendNotificationForAllUserValidation(sendNotificationForAllUserRequestViewModel);

				if (result.IsFailed)
					return result;

				var allUsersId =
					await DatabaseContext.Users
					.AsNoTracking()
					.Select(current => current.Id)
					.ToListAsync()
					;

				if (allUsersId == null || allUsersId.Count == 0)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserListEmpty);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				foreach (var userId in allUsersId)
				{
					await UnitOfWork.NotificationsRepository.AddAsync(new Notifications
					{
						From = sendNotificationForAllUserRequestViewModel.From,
						Title = sendNotificationForAllUserRequestViewModel.Title,
						Message = sendNotificationForAllUserRequestViewModel.Message,
						Direction = sendNotificationForAllUserRequestViewModel.Direction,
						UserId = userId.Value
					});
				}

				await UnitOfWork.SaveAsync();

				await HubContext.Clients.All.SendAsync("RefreshNotificationPanel");

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.NotificationSentSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				await Logger.LogCritical(exception: ex, message: ex.Message);

				var result =
					new Result<List<GetAllNotificationResponseViewModel>>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Result> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel)
		{
			try
			{
				var result =
					 SendNotificationForSpeceficUserValidation(sendNotificationForSpeceficUserRequestViewModel);

				if (result.IsFailed)
					return result;

				var user = 
					await DatabaseContext.Users
						.Where(current => current.Username.ToLower() == sendNotificationForSpeceficUserRequestViewModel.Username.ToLower())
						.FirstOrDefaultAsync()
						;

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				await DatabaseContext.Notifications.AddAsync(new Notifications
				{
					From = sendNotificationForSpeceficUserRequestViewModel.From,
					Title = sendNotificationForSpeceficUserRequestViewModel.Title,
					Message = sendNotificationForSpeceficUserRequestViewModel.Message,
					Direction = sendNotificationForSpeceficUserRequestViewModel.Direction,
					UserId = user.Id.Value,
				});

				await DatabaseContext.SaveChangesAsync();

				await HubContext.Clients.Group(user.Username.ToLower()).SendAsync("RefreshNotificationPanel");

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.NotificationSentSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				await Logger.LogCritical(exception: ex, message: ex.Message);

				var result =
					new Result<List<GetAllNotificationResponseViewModel>>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Result> AddTicketAsync(AddTicketRequestViewModel addTicketRequestViewModel)
		{
			try
			{
				var result =
					AddTicketValidation(addTicketRequestViewModel);

				if (result.IsFailed)
					return result;

				var user =
					HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken;

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var response =
					await DatabaseContext.Tickets.AddAsync(new Ticket()
					{
						Message = addTicketRequestViewModel.Message,
						Method = addTicketRequestViewModel.Method,
						UserId = user.Id
					});

				await DatabaseContext.SaveChangesAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.AddSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				await Logger.LogCritical(exception: ex, message: ex.Message);

				var result = new Result();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}
		#endregion /Methods
	}
}
