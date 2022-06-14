using EasyCaching.Core;

namespace Services
{
	public partial class UserServices : IUserServices
	{
		#region Constractor
		public UserServices
			(IMapper mapper,
			IUnitOfWork unitOfWork,
			IEasyCachingProvider cache,
			ITokenServices tokenUtility,
			DatabaseContext databaseContext,
			IOptions<ApplicationSettings> options,
			Dtat.Logging.ILogger<UserServices> logger,
			IHttpContextAccessor httpContextAccessor) : base()
		{
			Logger = logger;
			Mapper = mapper;
			UnitOfWork = unitOfWork;
			Cache = cache;
			TokenUtility = tokenUtility;
			ApplicationSettings = options.Value;
			DatabaseContext = databaseContext;
			HttpContextAccessor = httpContextAccessor;
		}
		#endregion /Constractor

		#region Properties
		public IMapper Mapper { get; }
		public IUnitOfWork UnitOfWork { get; }
		public IEasyCachingProvider Cache { get; }
		protected ITokenServices TokenUtility { get; }
		public DatabaseContext DatabaseContext { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		public Dtat.Logging.ILogger<UserServices> Logger { get; }
		protected ApplicationSettings ApplicationSettings { get; set; }
		#endregion /Properties

		#region Methods
		public async Task AddUserExistToCache(Guid userId)
		{
			await Cache.TrySetAsync
				($"userId-{userId}-exist", true, TimeSpan.FromHours(1));
		}


		public async Task<Result<UpdateUserProfileResponseViewModel>>
			UpdateUserAvatarAsync(IFormFile imageFile, IHostEnvironment HostEnvironment)
		{
			var result = CheckFileValidation(imageFile);

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
					(path: imageFile.FileName)?.ToLower();

			var rootPath =
				HostEnvironment.ContentRootPath;

			var newFileName = $"{Guid.NewGuid()}{fileExtension}";

			var physicalPathName =
				Path.Combine
					(path1: rootPath, path2: "wwwroot", path3: "ProfileImages", path4: newFileName);

			using (var stream = File.Create(path: physicalPathName))
			{
				await imageFile.CopyToAsync(target: stream);

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

			await Cache.RemoveByPrefixAsync($"userId-{user.Id.Value}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.UpdateSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result> UpdateUserAsync
			(UpdateUserRequestViewModel updateUserRequestViewModel)
		{
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

			if (user.Username != updateUserRequestViewModel.Username)
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


		public async Task<Result> DeleteUserAsync
			(DeleteUserRequestViewModel deleteUserRequestViewModel)
		{
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

			await Cache.RemoveByPrefixAsync($"userId-{existingUser.Id.Value}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.DeleteUserSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
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

			var userLogin =
			   await DatabaseContext.UserLogins
			   .Include(current => current.User)
			   .Where(current => current.RefreshToken == inputRefreshToken)
			   .FirstOrDefaultAsync();

			if (userLogin == null || userLogin.IsExpired)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidJwtToken);

				result.AddErrorMessage(errorMessage);
				return result;
			}

			var newRefreshToken = GenerateRefreshToken(ipAddress);
			userLogin.RefreshToken = newRefreshToken.RefreshToken;
			userLogin.CreatedByIp = ipAddress;

			await UnitOfWork.SaveAsync();

			var claims =
				new ClaimsIdentity(new[]
				{
					new Claim
						(type: nameof(userLogin.User.Id), value: userLogin.User.Id.ToString()),

					new Claim
						(type: nameof(userLogin.User.RoleId), value: userLogin.User.RoleId.ToString()),

					new Claim
						(type: nameof(userLogin.User.SecurityStamp), value: userLogin.User.SecurityStamp.ToString()),

					new Claim
						(type: nameof(userLogin.User.Username), value: userLogin.User.Username.ToString()),
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
					Username = userLogin.User.Username,
					Email = userLogin.User.Email,
					RefreshToken = newRefreshToken.RefreshToken,
				};

			result.Value = response;

			await AddUserExistToCache(userId: userLogin.User.Id.Value);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoginSuccessful);

			result.AddSuccessMessage(successMessage);

			Logger.LogWarning(Resources.Resource.UserRefreshTokenSuccessfulInformation, parameters: new List<object>
			{
				userLogin.User.Username,
			});

			return result;
		}


		public async Task<Result>
			DeleteUserProfileImageAsync(IHostEnvironment HostEnvironment)
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
				(Resources.Messages.SuccessMessages.DeleteAvatarImageSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
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

			var userLogin =
				await DatabaseContext.UserLogins
				.Include(current => current.User)
				.Where(current => current.RefreshToken == inputRefreshToken)
				.FirstOrDefaultAsync();

			if (userLogin == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserNotFound);

				result.AddErrorMessage(errorMessage);
				return result;
			}


			DatabaseContext.UserLogins.Remove(userLogin);

			await DatabaseContext.SaveChangesAsync();

			await Cache.RemoveByPrefixAsync($"userId-{userLogin.User.Id.Value}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.RefreshTokenRevoked);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result>
			RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
		{
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


		public async Task<Result<List<User>>> GetAllUsersAsync()
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


		public async Task<Result<LoginResponseViewModel>>
			LoginAsync(LoginRequestViewModel loginRequestViewModel, string ipAddress)
		{
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

						new Claim
							(type: nameof(foundedUser.Username), value: foundedUser.Username.ToString()),
				});

			string token =
				TokenUtility.GenerateJwtToken
					(securityKey: ApplicationSettings.JwtSettings.SecretKeyForToken,
					claimsIdentity: claims,
					dateTime: expiredTime);

			await AddUserExistToCache(userId: foundedUser.Id.Value);

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

			Logger.LogWarning(Resources.Resource.UserLoginSuccessfulInformation, parameters: new List<object>
			{
				new
				{
					Username = loginRequestViewModel.Username,
				}
			});

			return result;
		}


		public async Task<Result>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel)
		{
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

			if (result.IsFailed == true)
				return result;

			var isRoleExist =
				await DatabaseContext.Roles
				.Where(current => current.Id == changeUserRoleRequestViewModel.RoleId)
				.AnyAsync();

			if (!isRoleExist)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.RoleNotFound);

				result.AddErrorMessage(errorMessage);

				return result;
			}

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

			await Cache.RemoveByPrefixAsync($"userId-{foundedUser.Id.Value}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.UpdateSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result<GetUserInformationResponseViewModel>> GetUserInformationForUpdate()
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
				.Select(current => new { current.Id, current.Username, current.Email, current.PhoneNumber, current.ProfileImage })
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
		#endregion /Methods
	}
}
