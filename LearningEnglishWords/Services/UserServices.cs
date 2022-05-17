namespace Services
{
	public partial class UserServices : IUserServices
	{
		#region Constractor
		public UserServices
			(IMapper mapper,
			IUnitOfWork unitOfWork,
			ITokenUtility tokenUtility,
			DatabaseContext databaseContext,
			IOptions<ApplicationSettings> options,
			Dtat.Logging.ILogger<UserServices> logger,
			IHttpContextAccessor httpContextAccessor) : base()
		{
			Logger = logger;
			Mapper = mapper;
			UnitOfWork = unitOfWork;
			TokenUtility = tokenUtility;
            ApplicationSettings = options.Value;
			DatabaseContext = databaseContext;
			HttpContextAccessor = httpContextAccessor;
		}
		#endregion /Constractor

		#region Properties
		public IMapper Mapper { get; }
		public IUnitOfWork UnitOfWork { get; }
		protected ITokenUtility TokenUtility { get; }
        public DatabaseContext DatabaseContext { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		public Dtat.Logging.ILogger<UserServices> Logger { get; }
		protected ApplicationSettings ApplicationSettings { get; set; }
		#endregion /Properties

		#region Methods
		public async Task<Result<UpdateUserProfileResponseViewModel>>
			UpdateUserProfileAsync(IFormFile file, IHostEnvironment HostEnvironment)
		{
			try
			{
				var result = CheckFileValidation(file);

				if (result.IsFailed)
				{
					return result;
				}

				var userId =
					(HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken)?.Id;

				if (userId == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var user =
					await DatabaseContext.Users
					.Where(current => current.Id == userId)
					.FirstOrDefaultAsync();

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var fileExtension =
					Path.GetExtension
						(path: file.FileName)?.ToLower();

				var rootPath =
					HostEnvironment.ContentRootPath;

				var newFileName = $"{Guid.NewGuid()}{fileExtension}";

				var physicalPathName =
					Path.Combine
						(path1: rootPath, path2: "wwwroot", path3: "ProfileImages", path4: newFileName);

				using (var stream = File.Create(path: physicalPathName))
				{
					await file.CopyToAsync(target: stream);

					await stream.FlushAsync();

					stream.Close();
				}

				user.ProfileImage = $"/ProfileImages/{newFileName}";

				await DatabaseContext.SaveChangesAsync();

				result.Value = new UpdateUserProfileResponseViewModel
				{
					ProfileImage = user.ProfileImage,
				};

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.UpdateSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical(exception: ex, ex.Message);

				var response =
					new Result<UpdateUserProfileResponseViewModel>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		private UserLogin GenerateRefreshToken(string ipAddress)
		{
			return new UserLogin
			{
				RefreshToken = Guid.NewGuid(),
				Expires = DateTime.UtcNow.AddDays(30),
				Created = DateTime.UtcNow,
				CreatedByIp = ipAddress
			};
		}


		public async Task<User> GetByUsernameAsync(string username)
		{
			return
				await DatabaseContext
					.Users
					.AsNoTracking()
					.Where(current => current.Username.ToLower() == username.ToLower())
					.FirstOrDefaultAsync()
					;
		}


		public async Task<Result> UpdateUserByAdminAsync
			(UpdateUserByAdminRequestViewModel updateUserRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: updateUserRequestViewModel);

				await Logger.LogWarning
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result =
					 UpdateUserByAdminValidation(updateUserRequestViewModel: updateUserRequestViewModel);

				if (result.IsFailed == true)
					return result;

				var existingRole =
					await DatabaseContext.Roles
						.Where(current => current.Id == updateUserRequestViewModel.RoleId)
						.FirstOrDefaultAsync();

				if (existingRole == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.RoleNotFound);

					result.AddErrorMessage(errorMessage);
				}

				var existingUser =
					await UnitOfWork.UserRepository.GetByIdAsync(updateUserRequestViewModel.Id);

				if (existingUser == null)
				{
					string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);
					return result;
				}

				bool isEmailUpdate = true;
				bool isUsernameUpdate = true;

				if (existingUser.Email == updateUserRequestViewModel.Email)
					isEmailUpdate = false;

				if (existingUser.Username == updateUserRequestViewModel.Username)
					isUsernameUpdate = false;

				if (isEmailUpdate == true)
				{
					if (Validation.CheckEmailValid(updateUserRequestViewModel.Email) == false)
					{
						string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.InvalidEmailStructure);

						result.AddErrorMessage(errorMessage);
					}
					else
					{
						var isEmailExist =
							await UnitOfWork.UserRepository.CheckEmailExist(updateUserRequestViewModel.Email);

						if (isEmailExist == true)
						{
							string errorMessage = string.Format
								(Resources.Messages.ErrorMessages.EmailExist);

							result.AddErrorMessage(errorMessage);
						}
					}
				}

				if (isUsernameUpdate == true)
				{
					var isUsernameExist =
						await UnitOfWork.UserRepository.CheckUsernameExist(updateUserRequestViewModel.Username);

					if (isUsernameExist == true)
					{
						string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.UsernameExist);

						result.AddErrorMessage(errorMessage);
					}
				}

				if (result.IsFailed == true)
					return result;

				var user =
					Mapper.Map<User>(source: updateUserRequestViewModel);

				user.TimeUpdated = DateTime.UtcNow;

				await UnitOfWork.UserRepository.UpdateUserAsync(users: user);

				await UnitOfWork.SaveAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.UpdateSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await UnitOfWork.ClearTracking();

				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				var response = new Result();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result> UpdateUserAsync
			(UpdateUserRequestViewModel updateUserRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: updateUserRequestViewModel);

				await Logger.LogWarning
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result =
					 UpdateUserValidation(updateUserRequestViewModel: updateUserRequestViewModel);

				if (result.IsFailed == true)
					return result;

				var userId =
					(HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken)?.Id;

				if (userId == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var user =
					await DatabaseContext.Users
					.Where(current => current.Id == userId)
					.FirstOrDefaultAsync();

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				bool isChanged = false;

				if(user.Username != updateUserRequestViewModel.Username)
				{
					var isUsernameExist =
						await UnitOfWork.UserRepository.CheckUsernameExist(updateUserRequestViewModel.Username);

					if (isUsernameExist == true)
					{
						string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.UsernameExist);

						result.AddErrorMessage(errorMessage);
						return result;
					}

					user.Username = updateUserRequestViewModel.Username;
					isChanged = true;
				}

				if (updateUserRequestViewModel.PhoneNumber != null && 
					user.PhoneNumber != updateUserRequestViewModel.PhoneNumber)
				{
					user.PhoneNumber = updateUserRequestViewModel.PhoneNumber;
					isChanged = true;
				}

				if (isChanged)
				{
					await DatabaseContext.SaveChangesAsync();
				}

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.UpdateSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await UnitOfWork.ClearTracking();

				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				var response = new Result();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result> DeleteUsersAsync
			(DeleteUserRequestViewModel deleteUserRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: deleteUserRequestViewModel);

				await Logger.LogWarning
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result =
					DeleteUserValidation(deleteUserRequestViewModel: deleteUserRequestViewModel);

				if (result.IsFailed == true)
					return result;

				var existingUser =
					await UnitOfWork.UserRepository.GetByIdWithTrackingAsync(id: deleteUserRequestViewModel.Id);

				if (existingUser == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				existingUser.IsDeleted = true;
				existingUser.TimeUpdated = DateTime.UtcNow;

				await UnitOfWork.SaveAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.DeleteUserSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await UnitOfWork.ClearTracking();

				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				var response = new Result();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result<LoginResponseViewModel>>
			RefreshTokenAsync(string refreshToken, string ipAddress)
		{
			var result =
				 RefreshTokenValidation(refreshToken, ipAddress);

			if (result.IsFailed)
				return result;

			if (!Guid.TryParse(refreshToken, out var inputRefreshToken))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidJwtToken);

				result.AddErrorMessage(errorMessage);
				return result;
			}

            var userRefreshToken =
               await DatabaseContext.UserLogins
			   .Include(current => current.User)
			   .Where(current => current.RefreshToken == inputRefreshToken)
			   .FirstOrDefaultAsync();

			if (userRefreshToken == null || userRefreshToken.IsExpired)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidJwtToken);

				result.AddErrorMessage(errorMessage);
				return result;
			}

			var newRefreshToken = GenerateRefreshToken(ipAddress);
			userRefreshToken.RefreshToken = newRefreshToken.RefreshToken;
			userRefreshToken.CreatedByIp = ipAddress;

			await UnitOfWork.SaveAsync();

			var claims =
				new ClaimsIdentity(new[]
				{
					new Claim
						(type: nameof(userRefreshToken.User.Id), value: userRefreshToken.User.Id.ToString()),

					new Claim
						(type: nameof(userRefreshToken.User.RoleId), value: userRefreshToken.User.RoleId.ToString()),

					new Claim
						(type: nameof(userRefreshToken.User.SecurityStamp), value: userRefreshToken.User.SecurityStamp.ToString()),
				});

			var expiredTime =
				DateTime.UtcNow.AddMinutes(ApplicationSettings.JwtSettings.TokenExpiresTime);

			string jwtToken =
				TokenUtility.GenerateJwtToken
					(securityKey: ApplicationSettings.JwtSettings.SecretKeyForToken,
					claimsIdentity: claims,
					dateTime: expiredTime);

			LoginResponseViewModel response =
				new LoginResponseViewModel()
				{
					Token = jwtToken,
					Username = userRefreshToken.User.Username,
					Email = userRefreshToken.User.Email,
					RefreshToken = newRefreshToken.RefreshToken,
				};

			result.Value = response;

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoginSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result>
			DeleteUserProfileImageAsync(IHostEnvironment HostEnvironment)
		{
			try
			{
				var result = new Result();

				var userId =
					(HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken)?.Id;

				if (userId == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var user =
					await DatabaseContext.Users
					.Where(current => current.Id == userId)
					.FirstOrDefaultAsync();

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				if (!string.IsNullOrWhiteSpace(user.ProfileImage))
				{
					var rootPath =
						HostEnvironment.ContentRootPath;

					var physicalPathName =
						Path.Combine
							(path1: rootPath, path2: "wwwroot");

					try
					{
						File.Delete(path: $"{physicalPathName}{user.ProfileImage}");
					}
					catch { }

					user.ProfileImage = null;

					await DatabaseContext.SaveChangesAsync();
				}
				
				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.DeleteProfileImageSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await UnitOfWork.ClearTracking();

				await Logger.LogCritical(exception: ex, ex.Message);

				var response = new Result();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result> LogoutAsync(string token)
		{
			var result =
				 LogoutValidation(token);

			if (result.IsFailed)
				return result;

			if (!Guid.TryParse(token, out var inputRefreshToken))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidJwtToken);

				result.AddErrorMessage(errorMessage);
				return result;
			}

			var userRefreshToken =
				await DatabaseContext.UserLogins
				.Include(current => current.User)
				.Where(current => current.RefreshToken == inputRefreshToken)
				.FirstOrDefaultAsync();

			if (userRefreshToken == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserNotFound);

				result.AddErrorMessage(errorMessage);
				return result;
			}


			DatabaseContext.UserLogins.Remove(userRefreshToken);

			await DatabaseContext.SaveChangesAsync();

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.RefreshTokenRevoked);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result>
			RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: registerRequestViewModel);

				await Logger.LogInformation
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result =
					await RegisterValidation(registerRequestViewModel: registerRequestViewModel);

				if (result.IsFailed == true)
					return result;

				var user =
					Mapper.Map<User>(source: registerRequestViewModel);

				user.Password = Security.HashDataBySHA1(user.Password);

				user.SecurityStamp = Guid.NewGuid();

				await UnitOfWork.UserRepository.AddAsync(entity: user);

				await UnitOfWork.SaveAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.RegisterSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				var result = new Result();

				result.AddErrorMessage(errorMessage);

				return result;
			}
		}


		public async Task<Result<List<User>>> GetAllUsersAsync()
		{
			try
			{
				var result = 
					await DatabaseContext.Users
					.AsNoTracking()
					.ToListAsync()
					;

				var response =
					new Result<List<User>>();

				if (result != null)
				{
					if (result.Count > 0)
					{
						string successMessage = string.Format
							(Resources.Messages.SuccessMessages.LoadUsersSuccessfull);

						response.Value = result;

						response.AddSuccessMessage(successMessage);
					}
					else
					{
						string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.UserListEmpty);

						response.AddErrorMessage(errorMessage);
					}
				}
				else
				{
					string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.UserListEmpty);

					response.AddErrorMessage(errorMessage);
				}

				return response;
			}
			catch (Exception ex)
			{
				await UnitOfWork.ClearTracking();

				await Logger.LogCritical(exception: ex, ex.Message);

				var response =
					new Result<List<User>>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result<LoginResponseViewModel>>
			LoginAsync(LoginRequestViewModel loginRequestViewModel, string ipAddress)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: loginRequestViewModel);

				await Logger.LogWarning
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result =
					LoginValidation(loginRequestViewModel: loginRequestViewModel);

				if (result.IsFailed == true)
					return result;

				var hashedPassword =
					Security.HashDataBySHA1(loginRequestViewModel.Password);

				User foundedUser =
					await UnitOfWork.UserRepository.LoginAsync
						(username: loginRequestViewModel.Username, password: hashedPassword);

				if (foundedUser == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

					result.AddErrorMessage(errorMessage);
					return result;
				}

				var expiredTime =
					DateTime.UtcNow.AddMinutes(ApplicationSettings.JwtSettings.TokenExpiresTime);

				var refreshToken = GenerateRefreshToken(ipAddress);

				refreshToken.UserId = foundedUser.Id.Value;

				await DatabaseContext.UserLogins.AddAsync(refreshToken);

				await UnitOfWork.SaveAsync();

				var claims =
					new ClaimsIdentity(new[]
					{
						new Claim
							(type: nameof(foundedUser.Id), value: foundedUser.Id.ToString()),

						new Claim
							(type: nameof(foundedUser.RoleId), value: foundedUser.RoleId.ToString()),

						new Claim
							(type: nameof(foundedUser.SecurityStamp), value: foundedUser.SecurityStamp.ToString()),
					});

				string token =
					TokenUtility.GenerateJwtToken
						(securityKey: ApplicationSettings.JwtSettings.SecretKeyForToken,
						claimsIdentity: claims,
						dateTime: expiredTime);

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoginSuccessful);

				result.AddSuccessMessage(successMessage);

				LoginResponseViewModel response =
					new LoginResponseViewModel()
					{
						Token = token,
						Username = foundedUser.Username,
						Email = foundedUser.Email,
						RefreshToken = refreshToken.RefreshToken,
					};

				result.Value = response;

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				var result =
					new Result<LoginResponseViewModel>();

				result.AddErrorMessage(errorMessage);

				return result;
			}

		}


		public async Task<Result>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: changeUserRoleRequestViewModel);

				var result =
					ChangeUserRoleValidation(changeUserRoleRequestViewModel);

				var adminUser =
					(HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken)?.Id;

                if (adminUser == null)
                {
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				properties.Add("By Admin", adminUser);

				await Logger.LogWarning
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				if (result.IsFailed == true)
					return result;

				var foundedUser =
					await DatabaseContext.Users
					.Include(current => current.Role)
					.Where(current => current.Username.ToLower() == changeUserRoleRequestViewModel.Username.ToLower())
					.FirstOrDefaultAsync()
					;

				if (foundedUser == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);
					return result;
				}

				foundedUser.RoleId = changeUserRoleRequestViewModel.RoleId;
				foundedUser.SecurityStamp = Guid.NewGuid();

				await DatabaseContext.SaveChangesAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.UpdateSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				var result = new Result();

				if (ex.InnerException != null && ex.InnerException.Message.Contains("The conflict occurred"))
				{
					string InvalidRoleIdErrorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidRoleId);

					result.AddErrorMessage(InvalidRoleIdErrorMessage);

					return result;
				}

				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				result.AddErrorMessage(errorMessage);

				return result;
			}
		}


		public async Task<Result<GetUserInformationResponseViewModel>> GetUserInformationForUpdate()
		{
			try
			{
				var result = 
					new Result<GetUserInformationResponseViewModel>();

				var userId =
					(HttpContextAccessor?.HttpContext?.Items["User"] as UserInformationInToken)?.Id;

				if (userId == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var user =
					await DatabaseContext.Users
					.AsNoTracking()
					.Select(current => new {current.Id, current.Username, current.Email, current.PhoneNumber, current.ProfileImage})
					.Where(current => current.Id == userId)
					.FirstOrDefaultAsync();

				if (user == null)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				result.Value = new GetUserInformationResponseViewModel
				{
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Username = user.Username,
					ProfileImage = user.ProfileImage,
				};

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoadUserSuccessfull);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				var result =
					new Result<GetUserInformationResponseViewModel>();

				await Logger.LogCritical(exception: ex, ex.Message);

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				result.AddErrorMessage(errorMessage);

				return result;
			}
		}
		#endregion /Methods
	}
}
