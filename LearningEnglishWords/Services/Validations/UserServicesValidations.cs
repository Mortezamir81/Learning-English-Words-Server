using System.Threading.Tasks;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Services
{
	public partial class UserServices
	{
		#region Check Validation Methods
		//ForLoginValidation
		public Softmax.Results.Result<LoginResponseViewModel>
			LoginValidation(LoginRequestViewModel loginRequestViewModel)
		{
			var result =
				new Softmax.Results.Result<LoginResponseViewModel>();

			if (loginRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(loginRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(loginRequestViewModel.Username))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(loginRequestViewModel.Username), nameof(loginRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(loginRequestViewModel.Password))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(loginRequestViewModel.Password), nameof(loginRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			return result;

		}

		//ForRevokeTokenValidation
		public Softmax.Results.Result<LoginResponseViewModel>
			RefreshTokenValidation(string token, string ipAddress)
		{
			var result =
				new Softmax.Results.Result<LoginResponseViewModel>();

			if (string.IsNullOrWhiteSpace(token))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull, nameof(token));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(ipAddress))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull, nameof(ipAddress));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}

		//ForRefreshTokenValidation
		public Softmax.Results.Result
			LogoutValidation(string token)
		{
			var result =
				new Softmax.Results.Result<LoginResponseViewModel>();

			if (string.IsNullOrWhiteSpace(token))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull, nameof(token));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}

		//ChangeUserRoleValidation
		public Softmax.Results.Result
			ChangeUserRoleValidation(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel)
		{
			var result =
				new Softmax.Results.Result<LoginResponseViewModel>();

			if (changeUserRoleRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(changeUserRoleRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(changeUserRoleRequestViewModel.Username))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(changeUserRoleRequestViewModel.Username), nameof(changeUserRoleRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (changeUserRoleRequestViewModel.RoleId <= 0)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidRoleId);

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}

		//ForRegisterValidation
		public async Task<Softmax.Results.Result>
			RegisterValidation(RegisterRequestViewModel registerRequestViewModel)
		{
			var result =
				new Softmax.Results.Result();

			if (registerRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(registerRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(registerRequestViewModel.Username))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(registerRequestViewModel.Username), nameof(registerRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(registerRequestViewModel.Password))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(registerRequestViewModel.Password), nameof(registerRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(registerRequestViewModel.Email))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(registerRequestViewModel.Email), nameof(registerRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}
			else
			{
				if (Softmax.Utilities.Validation.CheckEmailValid(registerRequestViewModel.Email) == false)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidEmailStructure);

					result.AddErrorMessage(errorMessage);
				}
			}

			if (result.IsFailed == true)
				return result;

			var isEmailExist =
					await UnitOfWork.UserRepository.CheckEmailExist(registerRequestViewModel.Email);

			if (isEmailExist == true)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.EmailExist);

				result.AddErrorMessage(errorMessage);
			}

			var isUsernameExist =
				await UnitOfWork.UserRepository.CheckUsernameExist(registerRequestViewModel.Username);

			if (isUsernameExist == true)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UsernameExist);

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}

		//ForUpdateUserValidation
		public Softmax.Results.Result UpdateUserValidation
			(UpdateUserRequestViewModel updateUserRequestViewModel)
		{
			var result =
				new Softmax.Results.Result();

			if (updateUserRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(updateUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (updateUserRequestViewModel.Id == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.Id), nameof(updateUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(updateUserRequestViewModel.Username))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.Username), nameof(updateUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(updateUserRequestViewModel.Email))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.Email), nameof(updateUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}

		//ForDeleteUserValidation
		public Softmax.Results.Result DeleteUserValidation
			(DeleteUserRequestViewModel deleteUserRequestViewModel)
		{
			var result =
				new Softmax.Results.Result();

			if (deleteUserRequestViewModel.Id == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(deleteUserRequestViewModel.Id), nameof(deleteUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			return result;
		}
		#endregion /Check Validation Methods
	}
}
