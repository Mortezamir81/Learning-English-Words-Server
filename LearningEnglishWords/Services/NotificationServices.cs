using Persistence;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Requests;
using ViewModels.Responses;
using Dtat.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Services.SignalR;
using ViewModels.General;
using Microsoft.EntityFrameworkCore;

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
			ILogger<NotificationServices> logger,
			IHttpContextAccessor httpContextAccessor) : base()
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
		public ILogger<NotificationServices> Logger { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		#endregion /Properties

		#region Methods
		public async Task<Dtat.Results.Result
			<List<GetAllNotificationResponseViewModel>>> GetAllNotificationsAsync()
		{
			try
			{
				var result =
					new Dtat.Results.Result<List<GetAllNotificationResponseViewModel>>();

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
					new Dtat.Results.Result<List<GetAllNotificationResponseViewModel>>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Dtat.Results.Result> RemoveNotificationAsync(Guid notificationId)
		{
			try
			{
				var result =
					new Dtat.Results.Result();

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

				var result =
					new Dtat.Results.Result();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Dtat.Results.Result<ApplicationVersions>> GetLastVersionOfWPFAsync()
		{
			try
			{
				var result =
					new Dtat.Results.Result<ApplicationVersions>();

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
					new Dtat.Results.Result<ApplicationVersions>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Dtat.Results.Result> SendNotificationForAllUserAsync
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
					new Dtat.Results.Result<List<GetAllNotificationResponseViewModel>>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Dtat.Results.Result> SendNotificationForSpeceficUserAsync
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
					new Dtat.Results.Result<List<GetAllNotificationResponseViewModel>>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Dtat.Results.Result> AddTicketAsync(AddTicketRequestViewModel addTicketRequestViewModel)
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

				var result =
					new Dtat.Results.Result();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}
		#endregion /Methods
	}
}
