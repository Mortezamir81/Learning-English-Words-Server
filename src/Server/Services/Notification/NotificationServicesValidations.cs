namespace Services
{
	public partial class NotificationServices
	{
		#region Check Validation Methods
		public Result AddTicketValidation
			(AddTicketRequestViewModel addTicketRequestViewModel)
		{
			var result = new Result();

			if (string.IsNullOrWhiteSpace(addTicketRequestViewModel.Message))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addTicketRequestViewModel.Message), nameof(addTicketRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(addTicketRequestViewModel.Method))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addTicketRequestViewModel.Method), nameof(addTicketRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}


		public Result SendNotificationForAllUserValidation
			(SendNotificationForAllUserRequestViewModel sendNotificationForAllUserRequestViewModel)
		{
			var result = new Result();

			if (sendNotificationForAllUserRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(sendNotificationForAllUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(sendNotificationForAllUserRequestViewModel.Message))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(sendNotificationForAllUserRequestViewModel.Message), nameof(sendNotificationForAllUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(sendNotificationForAllUserRequestViewModel.Title))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(sendNotificationForAllUserRequestViewModel.Title), nameof(sendNotificationForAllUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(sendNotificationForAllUserRequestViewModel.From))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(sendNotificationForAllUserRequestViewModel.From), nameof(sendNotificationForAllUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (!string.IsNullOrWhiteSpace(sendNotificationForAllUserRequestViewModel.Direction))
			{
				if (sendNotificationForAllUserRequestViewModel.Direction == "ltr" ||
					sendNotificationForAllUserRequestViewModel.Direction == "rtl")
				{

				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidDirectionValue);

					result.AddErrorMessage(errorMessage);
				}
			}

			return result;
		}


		public Result SendNotificationForSpeceficUserValidation
			(SendNotificationForSpeceficUserRequestViewModel sendNotificationForSpeceficUserRequestViewModel)
		{
			var result = new Result();

			if (sendNotificationForSpeceficUserRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(sendNotificationForSpeceficUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(sendNotificationForSpeceficUserRequestViewModel.Message))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(sendNotificationForSpeceficUserRequestViewModel.Message), nameof(sendNotificationForSpeceficUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(sendNotificationForSpeceficUserRequestViewModel.Title))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(sendNotificationForSpeceficUserRequestViewModel.Title), nameof(sendNotificationForSpeceficUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(sendNotificationForSpeceficUserRequestViewModel.From))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(sendNotificationForSpeceficUserRequestViewModel.From), nameof(sendNotificationForSpeceficUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (!string.IsNullOrWhiteSpace(sendNotificationForSpeceficUserRequestViewModel.Direction))
			{
				if (sendNotificationForSpeceficUserRequestViewModel.Direction == "ltr" ||
					sendNotificationForSpeceficUserRequestViewModel.Direction == "rtl")
				{

				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidDirectionValue);

					result.AddErrorMessage(errorMessage);
				}
			}

			if (string.IsNullOrWhiteSpace(sendNotificationForSpeceficUserRequestViewModel.UserName))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(sendNotificationForSpeceficUserRequestViewModel.UserName), nameof(sendNotificationForSpeceficUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}
		#endregion /Check Validation Methods
	}
}
