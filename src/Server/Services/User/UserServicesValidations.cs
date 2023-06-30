namespace Services
{
	public partial class UserServices
	{
		#region Check Validation Methods
		public Result
			LogoutValidation(string token)
		{
			var result =
				new Result<LoginResponseViewModel>();

			if (string.IsNullOrWhiteSpace(token))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull, nameof(token));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}


		public Result DeleteUserValidation
			(DeleteUserRequestViewModel deleteUserRequestViewModel)
		{
			var result = new Result();

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


		public Result<LoginResponseViewModel>
			RefreshTokenValidation(string token, string ipAddress)
		{
			var result =
				new Result<LoginResponseViewModel>();

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


		public Result UpdateUserValidation
			(UpdateUserRequestViewModel updateUserRequestViewModel)
		{
			var result = new Result();

			if (updateUserRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
						nameof(updateUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(updateUserRequestViewModel.UserName))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.UserName), nameof(updateUserRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}


		public Result<LoginResponseViewModel>
			LoginValidation(LoginRequestViewModel loginRequestViewModel)
		{
			var result =
				new Result<LoginResponseViewModel>();

			if (loginRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(loginRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(loginRequestViewModel.UserName))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(loginRequestViewModel.UserName), nameof(loginRequestViewModel));

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


		public Result UpdateUserByAdminValidation
			(UpdateUserByAdminRequestViewModel updateUserRequestViewModel)
		{
			var result = new Result();

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

			if (string.IsNullOrWhiteSpace(updateUserRequestViewModel.UserName))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.UserName), nameof(updateUserRequestViewModel));

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


		public async Task<Result>
			RegisterValidation(RegisterRequestViewModel registerRequestViewModel)
		{
			var result = new Result();

			if (registerRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(registerRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(registerRequestViewModel.UserName))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(registerRequestViewModel.UserName), nameof(registerRequestViewModel));

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
				if (Dtat.Utilities.Validation.CheckEmailValid(registerRequestViewModel.Email) == false)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidEmailStructure);

					result.AddErrorMessage(errorMessage);
				}
			}

			if (!result.IsSuccess == true)
				return result;

			var isEmailExist =
					await UnitOfWork.UserRepository.CheckEmailExist(registerRequestViewModel.Email);

			if (isEmailExist == true)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.EmailExist);

				result.AddErrorMessage(errorMessage);
			}

			var isUserNameExist =
				await UnitOfWork.UserRepository.CheckUserNameExist(registerRequestViewModel.UserName);

			if (isUserNameExist == true)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserNameExist);

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}


		public Result
			ChangeUserRoleValidation(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel)
		{
			var result =
				new Result<LoginResponseViewModel>();

			if (changeUserRoleRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(changeUserRoleRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			return result;
		}


		public Result<UpdateUserProfileResponseViewModel> CheckFileValidation(IFormFile file)
		{
			var result =
				new Result<UpdateUserProfileResponseViewModel>();

			if (file == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.FileNull);

				result.AddErrorMessage(errorMessage);

				return result;
			}


			if (file.Length == 0)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.FileNotUploaded, file.FileName);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var fileExtension =
				System.IO.Path.GetExtension
				(path: file.FileName)?.ToLower();

			if (fileExtension == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.FileDoesNotHaveExtention, file.FileName);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var permittedFileExtensions =
				new string[] { ".ico", ".png", ".jpg", ".jpeg" };

			if (permittedFileExtensions.ToList().Contains(item: fileExtension) == false)
			{
				var errorMessage = string.Format
					(Resources.Messages.ErrorMessages.FileExtentionDoesNotSupport, file.FileName);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			return result;
		}
		#endregion /Check Validation Methods
	}
}
