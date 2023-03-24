namespace Services.SignalR
{
	[Authorize]
	public class SignalHub : Hub
	{
		#region Constractor
		public SignalHub
			(IMapper mapper,
			IUnitOfWork unitOfWork,
			DatabaseContext databaseContext,
			Dtat.Logging.ILogger<SignalHub> logger,
			IHttpContextAccessor httpContextAccessor) : base()
		{
			Mapper = mapper;
			Logger = logger;
			UnitOfWork = unitOfWork;
			DatabaseContext = databaseContext;
			HttpContextAccessor = httpContextAccessor;
		}
		#endregion

		#region Properties
		public IMapper Mapper { get; }
		public IUnitOfWork UnitOfWork { get; }
		public DatabaseContext DatabaseContext { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		public Dtat.Logging.ILogger<SignalHub> Logger { get; set; }
		#endregion

		#region Methods
		public override async Task OnConnectedAsync()
		{
			var userId = GetUserId();

			if (userId != null)
			{
				Logger.LogDebug($"User {userId} connected successful");

				await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString()!);

				await Clients.All.SendAsync("RefreshNotificationPanel");
			}

			await base.OnConnectedAsync();
		}


		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = GetUserId();

			if (userId != null)
			{
				Logger.LogDebug($"User {userId} disconnected successful");
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString()!);
			}

			await base.OnDisconnectedAsync(exception);
		}


		public async Task ChangeIsReadNotificationToTrue(Guid notificationId)
		{
			var userId = GetUserId();

			if (userId == null)
				return;

			Logger.LogDebug($"notif is read by {userId}");

			var notification =
				DatabaseContext.Notifications
				.Where(current => current.Id == notificationId)
				.Where(current => current.UserId == userId)
				.FirstOrDefault();

			if (notification == null)
				return;

			notification.IsRead = true;
			await UnitOfWork.SaveAsync();

			Logger.LogDebug($"notification read updated in databas done.");
		}


		public async Task<Result<List<GetWordResponseViewModel>>> SearchWord(string word)
		{
			var result =
				new Result<List<GetWordResponseViewModel>>();

			var userId = GetUserId();

			if (userId == null)
			{
				result.AddErrorMessage(nameof(HttpStatusCode.Unauthorized));

				return result;
			}

			var foundedWords =
				await UnitOfWork.WordsRepository.SearchWordAsync(word, userId.Value);

			if (foundedWords == null || foundedWords.Count <= 0)
			{
				string wordsListErrorMessage = string.Format
					(Resources.Messages.ErrorMessages.WordsListEmpty);

				result.AddErrorMessage(wordsListErrorMessage);

				return result;
			}

			var wordsList = new List<GetWordResponseViewModel>();

			foreach (var foundedWord in foundedWords)
			{
				var responseWord =
					Mapper.Map<GetWordResponseViewModel>(source: foundedWord);

				wordsList.Add(responseWord);
			}

			result.Value = wordsList;

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoadWordSuccessfull);

			result.AddSuccessMessage(message: successMessage);

			return result;
		}


		private Guid? GetUserId()
		{
			if (HttpContextAccessor?.HttpContext?.User.Identity?.IsAuthenticated == false)
				return default;

			var stringUserId =
				HttpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(current => current.Type == ClaimTypes.NameIdentifier)?.Value;

			if (stringUserId == null)
				return default;

			var userId = Guid.Parse(stringUserId);

			return userId;
		}
		#endregion
	}
}
