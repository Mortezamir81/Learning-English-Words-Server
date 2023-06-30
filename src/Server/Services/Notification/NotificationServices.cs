namespace Services
{
	public partial class NotificationServices : INotificationServices, IRegisterAsScoped
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
			<GetAllNotificationResponseViewModel>>> GetAllNotificationsAsync(Guid userId)
		{
			var result =
				new Result<List<GetAllNotificationResponseViewModel>>();

			var notifications =
				await UnitOfWork.NotificationsRepository.GetAllNotification(userId: userId);

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


		public async Task<Result> RemoveNotificationAsync(Guid notificationId, Guid userId)
		{
			var result = new Result();

			var response =
				await UnitOfWork.NotificationsRepository.GetNotification(notificationId: notificationId, userId: userId);

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


		public async Task<Result<ApplicationVersion>> GetLastVersionOfWPFAsync()
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


		public async Task<Result> SendNotificationForAllUserAsync
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel)
		{
			var result =
				 SendNotificationForAllUserValidation(sendNotificationForAllUserRequestViewModel);

			if (!result.IsSuccess)
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
					UserId = userId
				});
			}

			await UnitOfWork.SaveAsync();

			await HubContext.Clients.All.SendAsync("RefreshNotificationPanel");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.NotificationSentSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result> SendNotificationForSpeceficUserAsync
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel)
		{
			var result =
				 SendNotificationForSpeceficUserValidation(sendNotificationForSpeceficUserRequestViewModel);

			if (!result.IsSuccess)
				return result;

			var user =
				await DatabaseContext.Users
					.Where(current => current.UserName.ToLower() == sendNotificationForSpeceficUserRequestViewModel.UserName.ToLower())
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
				UserId = user.Id,
			});

			await DatabaseContext.SaveChangesAsync();

			await HubContext.Clients.Group(user.UserName.ToLower()).SendAsync("RefreshNotificationPanel");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.NotificationSentSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result> AddTicketAsync(AddTicketRequestViewModel addTicketRequestViewModel, Guid userId)
		{
			var result =
				AddTicketValidation(addTicketRequestViewModel);

			if (!result.IsSuccess)
				return result;

			var response =
				await DatabaseContext.Tickets.AddAsync(new Ticket()
				{
					Message = addTicketRequestViewModel.Message,
					Method = addTicketRequestViewModel.Method,
					UserId = userId
				});

			await DatabaseContext.SaveChangesAsync();

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.AddSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}
		#endregion /Methods
	}
}
