using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Domain.Entities;
using Persistence;
using System;
using System.Threading.Tasks;
using Softmax.Results;
using System.Collections.Generic;
using ViewModels.Responses;
using ViewModels.General;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Softmax.Logging;
using Infrustructrue.Attributes;
using Infrustructrue.Enums;

namespace Services.SignalR
{
	public class SignalHub : Hub
	{
		public SignalHub
			(IMapper mapper,
			IUnitOfWork unitOfWork,
			ILogger<SignalHub> logger,
			DatabaseContext databaseContext,
			IHttpContextAccessor httpContextAccessor) : base()
		{
			Mapper = mapper;
			Logger = logger;
			UnitOfWork = unitOfWork;
			DatabaseContext = databaseContext;
			HttpContextAccessor = httpContextAccessor;
		}


		public IMapper Mapper { get; }
		public IUnitOfWork UnitOfWork { get; }
		public ILogger<SignalHub> Logger { get; set; }
		public DatabaseContext DatabaseContext { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }


		public override async Task OnConnectedAsync()
		{
			var user = HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;

			if (user != null)
			{
				await Logger.LogTrace($"User {user.Username} connected successful");
				await Groups.AddToGroupAsync(Context.ConnectionId, user.Username.ToLower());
				await Clients.All.SendAsync("RefreshNotificationPanel");
			}

			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var user = HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;

			if (user != null)
			{
				await Logger.LogTrace($"User {user.Username} disconnected successful");
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Username.ToLower());
			}

			await base.OnDisconnectedAsync(exception);
		}

		public async Task ChangeIsReadNotificationToTrue(Guid notificationId)
		{
			var user = HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;

			if (user != null)
			{
				await Logger.LogTrace($"notif is read by {user.Username}");

				var result =
					await DatabaseContext.Users
					.Include(current => current.Notifications)
					.Where(current => current.Username.ToLower() == user.Username.ToLower())
					.FirstOrDefaultAsync()
					;

                if (result != null)
                {
					await Logger.LogTrace($"user : {user.Username} is not null and readed by database");

					foreach (var notification in result.Notifications)
					{
						if (notification.Id == notificationId)
						{
							notification.IsRead = true;
							await UnitOfWork.SaveAsync();

							await Logger.LogTrace($"notification read updated in databas done.");
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

			var user = HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;

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
	}
}
