namespace Services.SignalR
{
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
            var user = HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken;

            if (user != null)
            {
                Logger.LogDebug($"User {user.Id} connected successful");
                await Groups.AddToGroupAsync(Context.ConnectionId, user.Id.ToString());
                await Clients.All.SendAsync("RefreshNotificationPanel");
            }

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken;

            if (user != null)
            {
                Logger.LogDebug($"User {user.Id} disconnected successful");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Id.ToString());
            }

            await base.OnDisconnectedAsync(exception);
        }


        public async Task ChangeIsReadNotificationToTrue(Guid notificationId)
        {
            var user = HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken;

            if (user != null)
            {
                Logger.LogDebug($"notif is read by {user.Id}");

                var result =
                    await DatabaseContext.Users
                    .Include(current => current.Notifications)
                    .Where(current => current.Id == user.Id)
                    .FirstOrDefaultAsync()
                    ;

                if (result != null)
                {
                    Logger.LogDebug($"user : {user.Id} is not null and readed by database");

                    foreach (var notification in result.Notifications)
                    {
                        if (notification.Id == notificationId)
                        {
                            notification.IsRead = true;
                            await UnitOfWork.SaveAsync();

                            Logger.LogDebug($"notification read updated in databas done.");
                            break;
                        }
                    }
                }
            }
        }


        public async Task<Result<List<GetWordResponseViewModel>>> SearchWord(string word)
        {
            var result =
                new Result<List<GetWordResponseViewModel>>();

            var user = HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken;

            if (user != null)
            {
                var foundedWords =
                    await UnitOfWork.WordsRepository.SearchWordAsync(word, user.Id);

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
                    GetWordResponseViewModel responseWord =
                        Mapper.Map<GetWordResponseViewModel>(source: foundedWord);

                    wordsList.Add(responseWord);
                }

                result.Value = wordsList;

                string successMessage = string.Format
                    (Resources.Messages.SuccessMessages.LoadWordSuccessfull);

                result.AddSuccessMessage(message: successMessage);

                return result;
            }

            result.AddErrorMessage("Unauthorized");

            return result;
        }
        #endregion
    }
}
