using Microsoft.AspNetCore.Identity;

namespace Services
{
	public partial class UserServices : IUserServices, IRegisterAsScoped
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
			UserManager<User> userManager,
			RoleManager<Role> roleManager) : base()
		{
			Logger = logger;
			Mapper = mapper;
			UnitOfWork = unitOfWork;
			Cache = cache;
			TokenUtility = tokenUtility;
			ApplicationSettings = options.Value;
			DatabaseContext = databaseContext;
			UserManager = userManager;
			RoleManager = roleManager;
		}
		#endregion /Constractor

		#region Properties
		public IMapper Mapper { get; }
		public IUnitOfWork UnitOfWork { get; }
		public IEasyCachingProvider Cache { get; }
		protected ITokenServices TokenUtility { get; }
		public DatabaseContext DatabaseContext { get; }
		public UserManager<User> UserManager { get; }
		public RoleManager<Role> RoleManager { get; }
		public Dtat.Logging.ILogger<UserServices> Logger { get; }
		protected ApplicationSettings ApplicationSettings { get; set; }
		#endregion /Properties

		#region Methods
		public async Task<Result<UpdateUserProfileResponseViewModel>>
			UpdateUserAvatarAsync(IFormFile imageFile, IHostEnvironment HostEnvironment, Guid userId)
		{
			var result = CheckFileValidation(imageFile);

			if (!result.IsSuccess)
			{
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

			if (!string.IsNullOrWhiteSpace(user.ProfileImage))
			{
				var pathForDeleteFile =
					Path.Combine
						(path1: rootPath, path2: "wwwroot",
						path3: user.ProfileImage.Split("/")?[0],
						path4: user.ProfileImage.Split("/")?[1]);

				if (File.Exists(pathForDeleteFile))
				{
					File.Delete(path: pathForDeleteFile);
				}
			}

			var pathForAddFile =
				Path.Combine
					(path1: rootPath, path2: "wwwroot", path3: "ProfileImages", path4: newFileName);

			using (var stream = File.Create(path: pathForAddFile))
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


		public async Task<User> GetByUserNameAsync(string userName)
		{
			return
				await DatabaseContext
					.Users
					.AsNoTracking()
					.Where(current => current.UserName.ToLower() == userName.ToLower())
					.FirstOrDefaultAsync()
					;
		}


		public async Task<Result> UpdateUserByAdminAsync
			(UpdateUserByAdminRequestViewModel updateUserRequestViewModel)
		{
			var result =
				 UpdateUserByAdminValidation(updateUserRequestViewModel: updateUserRequestViewModel);

			if (!result.IsSuccess == true)
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
			bool isUserNameUpdate = true;

			if (existingUser.Email == updateUserRequestViewModel.Email)
				isEmailUpdate = false;

			if (existingUser.UserName == updateUserRequestViewModel.UserName)
				isUserNameUpdate = false;

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

			if (isUserNameUpdate == true)
			{
				var isUserNameExist =
					await UnitOfWork.UserRepository.CheckUserNameExist(updateUserRequestViewModel.UserName);

				if (isUserNameExist == true)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNameExist);

					result.AddErrorMessage(errorMessage);
				}
			}

			if (!result.IsSuccess == true)
				return result;

			var user =
				Mapper.Map<User>(source: updateUserRequestViewModel);

			user.TimeUpdated = DateTime.UtcNow;

			await UnitOfWork.UserRepository.UpdateUserAsync(users: user);

			await UnitOfWork.SaveAsync();

			await Cache.RemoveByPrefixAsync($"userId-{user.Id}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.UpdateSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result> UpdateUserAsync
			(UpdateUserRequestViewModel updateUserRequestViewModel, Guid userId)
		{
			var result =
				 UpdateUserValidation(updateUserRequestViewModel: updateUserRequestViewModel);

			if (!result.IsSuccess == true)
				return result;

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

			if (user.UserName != updateUserRequestViewModel.UserName)
			{
				var isUserNameExist =
					await UnitOfWork.UserRepository.CheckUserNameExist(updateUserRequestViewModel.UserName);

				if (isUserNameExist == true)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNameExist);

					result.AddErrorMessage(errorMessage);
					return result;
				}

				user.UserName = updateUserRequestViewModel.UserName;
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

			if (!result.IsSuccess == true)
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

			await Cache.RemoveByPrefixAsync($"userId-{existingUser.Id}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.DeleteUserSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		public async Task<Result<LoginResponseViewModel>>
			RefreshTokenAsync(string refreshToken, string? ipAddress)
		{
			var result =
				 new Result<LoginResponseViewModel>();

			if (!Guid.TryParse(refreshToken, out var inputRefreshToken))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidRefreshToken);

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
					(Resources.Messages.ErrorMessages.InvalidRefreshToken);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			if (userLogin.User == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidRefreshToken);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			if (userLogin.User.IsBanned)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserBanned);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var newRefreshToken = Guid.NewGuid();

			userLogin.RefreshToken = newRefreshToken;
			userLogin.CreatedByIp = ipAddress;

			await DatabaseContext.SaveChangesAsync();

			var claimsIdentity =
				await CreateClaimsIdentity(user: userLogin.User);

			var accessToken =
				CreateAccessToken(claimsIdentity: claimsIdentity);

			var response =
				new LoginResponseViewModel()
				{
					Token = accessToken,
					UserName = userLogin!.User?.UserName,
					RefreshToken = newRefreshToken,
				};

			result.Value = response;

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.RefreshTokenSuccessfull);

			result.AddSuccessMessage(successMessage);

			await AddUserLoggedInToCache(userLogin.User!.Id);

			Logger.LogInformation(Resources.Resource.UserRefreshTokenSuccessfulInformation, parameters: new List<object?>
			{
				userLogin.User?.UserName,
			});

			return result;
		}


		public async Task<Result>
			DeleteUserProfileImageAsync(IHostEnvironment HostEnvironment, Guid userId)
		{
			var result = new Result();

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

				var physicalPath =
					Path.Combine
						(path1: rootPath, path2: "wwwroot",
						path3: user.ProfileImage.Split("/")?[0],
						path4: user.ProfileImage.Split("/")?[1]);

				if (File.Exists(physicalPath))
				{
					File.Delete(path: physicalPath);
				}

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

			if (!result.IsSuccess)
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

			await Cache.RemoveByPrefixAsync($"userId-{userLogin.User.Id}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.RefreshTokenRevoked);

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


		/// <summary>
		/// Login a user by username and password
		/// </summary>
		/// <param name="requestViewModel"></param>
		/// <param name="ipAddress"></param>
		/// <returns>Success or Failed Result</returns>
		public async Task<Result<LoginResponseViewModel>>
			LoginAsync(LoginRequestViewModel requestViewModel, string? ipAddress)
		{
			var result =
				new Result<LoginResponseViewModel>();

			var foundedUser =
				await GetUserByName(requestViewModel.UserName!);

			if (foundedUser == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var isPasswordValid =
				await CheckPasswordValid(user: foundedUser, password: requestViewModel.Password!);

			if (!isPasswordValid)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			if (foundedUser.IsBanned)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserBanned);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var refreshToken =
				await AddRefreshTokenToDB(userId: foundedUser.Id, ipAddress: ipAddress);

			var claimsIdentity =
				await CreateClaimsIdentity(user: foundedUser);

			var accessToken =
				CreateAccessToken(claimsIdentity: claimsIdentity);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoginSuccessful);

			result.AddSuccessMessage(successMessage);

			var response =
				new LoginResponseViewModel()
				{
					Token = accessToken,
					UserName = foundedUser.UserName,
					RefreshToken = refreshToken,
				};

			result.Value = response;

			await AddUserLoggedInToCache(foundedUser.Id);

			Logger.LogInformation(Resources.Resource.UserLoginSuccessfulInformation, parameters: new List<object>
			{
				new
				{
					UserName = requestViewModel.UserName,
				}
			});

			return result;
		}


		/// <summary>
		/// Login a user by OAuth standard Authentication (for use in swagger ui)
		/// </summary>
		/// <param name="requestViewModel"></param>
		/// <param name="ipAddress"></param>
		/// <returns>Success or Failed Result</returns>
		public async Task<Result<LoginByOAuthResponseViewModel>> LoginByOAuthAsync
			(LoginByOAuthRequestViewModel requestViewModel, string? ipAddress)
		{
			var result =
				new Result<LoginByOAuthResponseViewModel>();

			if (!requestViewModel.Grant_Type?.Equals("password", StringComparison.OrdinalIgnoreCase) == true)
			{
				string errorMessage = "OAuth flow is not password.";

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var foundedUser =
				await GetUserByName(requestViewModel.UserName!);

			if (foundedUser == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var isPasswordValid =
				await CheckPasswordValid(user: foundedUser, password: requestViewModel.Password!);

			if (!isPasswordValid)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			if (foundedUser.IsBanned)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserBanned);

				result.AddErrorMessage(errorMessage);

				return result;
			}

			var refreshToken =
				await AddRefreshTokenToDB(userId: foundedUser.Id, ipAddress: ipAddress);

			var claimsIdentity =
				await CreateClaimsIdentity(user: foundedUser);

			var accessToken =
				CreateAccessToken(claimsIdentity: claimsIdentity);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoginSuccessful);

			result.AddSuccessMessage(successMessage);

			var response =
				new LoginByOAuthResponseViewModel()
				{
					access_token = accessToken,
					username = foundedUser.UserName,
					refresh_token = refreshToken.ToString(),
					token_type = "Bearer",
				};

			result.Value = response;

			await AddUserLoggedInToCache(foundedUser.Id);

			Logger.LogInformation(Resources.Resource.UserLoginSuccessfulInformation, parameters: new List<object>
			{
				new
				{
					requestViewModel.UserName,
				}
			});

			return result;
		}


		/// <summary>
		/// Create a new user in database
		/// </summary>
		/// <param name="registerRequestViewModel"></param>
		/// <returns>Success or Failed Result</returns>
		public async Task<Result>
			RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
		{
			var result = new Result();

			var registerUser =
				new User(registerRequestViewModel.UserName!)
				{
					Email = registerRequestViewModel.Email,
				};

			var identityUserResult =
				await UserManager.CreateAsync(registerUser, registerRequestViewModel.Password!);

			if (!identityUserResult.Succeeded)
			{
				foreach (var error in identityUserResult.Errors)
				{
					result.AddErrorMessage(error.Description);
				}

				return result;
			}

			var identityRoleResult =
				await UserManager.AddToRoleAsync(registerUser, Constants.Role.User);

			if (!identityRoleResult.Succeeded)
			{
				foreach (var error in identityUserResult.Errors)
				{
					result.AddErrorMessage(error.Description);
				}

				return result;
			}

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.RegisterSuccessful);

			result.AddSuccessMessage(successMessage);

			return result;
		}


		/// <summary>
		/// Change user role by admin
		/// </summary>
		/// <param name="requestViewModel"></param>
		/// <param name="adminId"></param>
		/// <returns>Success or Failed Result</returns>
		public async Task<Result> ChangeUserRoleAsync
			(ChangeUserRoleRequestViewModel requestViewModel, Guid adminId)
		{
			var result = new Result();

			var adminUser =
				await UserManager.FindByIdAsync(adminId.ToString());

			if (adminUser == null)
			{
				var errorMessage =
					string.Format(nameof(HttpStatusCode.Unauthorized));

				result.AddErrorMessage(errorMessage, MessageCode.HttpUnauthorizeError);

				return result;
			}

			var isRoleExist =
				await RoleManager.RoleExistsAsync(requestViewModel.RoleName);

			if (!isRoleExist)
			{
				var errorMessage = string.Format
					(Resources.Messages.ErrorMessages.RoleNotFound);

				result.AddErrorMessage(errorMessage, MessageCode.HttpNotFoundError);

				return result;
			}

			var adminRoles =
				await UserManager.GetRolesAsync(adminUser);

			var foundedUser =
				await UserManager.FindByIdAsync
					(userId: requestViewModel.UserId.ToString());

			if (foundedUser == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UserNotFound);

				result.AddErrorMessage(errorMessage, MessageCode.HttpNotFoundError);

				return result;
			}

			var userRoles =
				await UserManager.GetRolesAsync(foundedUser);

			if (adminRoles.Count == 0 || userRoles.Count == 0)
			{
				var errorMessage = string.Format
					(Resources.Messages.ErrorMessages.AccessDeniedForChangeRole);

				result.AddErrorMessage(errorMessage, MessageCode.HttpForbidenError);

				return result;
			}

			if (foundedUser.Id == adminId)
			{
				var errorMessage = string.Format
					(Resources.Messages.ErrorMessages.AccessDeniedForChangeRole);

				result.AddErrorMessage(errorMessage, MessageCode.HttpForbidenError);

				return result;
			}

			if (foundedUser.IsSystemic)
			{
				var errorMessage = string.Format
					(Resources.Messages.ErrorMessages.AccessDeniedForChangeRole);

				result.AddErrorMessage(errorMessage, MessageCode.HttpForbidenError);

				return result;
			}

			foreach (var role in userRoles)
			{
				await UserManager.RemoveFromRoleAsync(foundedUser, role);
			}

			var addToRoleResult =
				await UserManager.AddToRoleAsync(foundedUser, requestViewModel.RoleName);

			if (!addToRoleResult.Succeeded)
			{
				foreach (var error in addToRoleResult.Errors)
				{
					result.AddErrorMessage(error.Description);
				}

				return result;
			}

			await UserManager.UpdateSecurityStampAsync(foundedUser);

			await Cache.RemoveByPrefixAsync($"userId-{foundedUser.Id}");

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.UpdateSuccessful);

			result.AddSuccessMessage(successMessage);

			Logger.LogWarning(Resources.Resource.UpdateSuccessful, parameters: new List<object>
			{
				requestViewModel
			});

			return result;
		}


		public async Task<Result<GetUserInformationResponseViewModel>> GetUserInformationForUpdate(Guid userId)
		{
			var result =
				new Result<GetUserInformationResponseViewModel>();

			var user =
				await DatabaseContext.Users
				.AsNoTracking()
				.Select(current => new { current.Id, current.UserName, current.Email, current.PhoneNumber, current.ProfileImage })
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
				UserName = user.UserName,
				ProfileImage = user.ProfileImage,
			};

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoadUserSuccessfull);

			result.AddSuccessMessage(successMessage);

			return result;
		}
		#endregion /Methods

		#region Private Methods
		/// <summary>
		/// Generate new refresh token
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <returns>Success or Failed Result</returns>
		private UserLogin GenerateRefreshToken(string? ipAddress)
		{
			return new UserLogin(refreshToken: Guid.NewGuid())
			{
				Expires = DateTime.UtcNow.AddDays(30),
				Created = DateTime.UtcNow,
				CreatedByIp = ipAddress
			};
		}

		private string CreateAccessToken(ClaimsIdentity claimsIdentity)
		{
			var expiredTime =
				DateTime.UtcNow.AddMinutes(ApplicationSettings.JwtSettings?.TokenExpiresTime ?? 15);
			var accessToken =
			TokenUtility.GenerateJwtToken
					(securityKey: ApplicationSettings.JwtSettings!.SecretKeyForToken,
					claimsIdentity: claimsIdentity,
					dateTime: expiredTime);

			return accessToken;
		}

		private async Task<User?> GetUserByName(string name)
		{
			return await UserManager.FindByNameAsync(name);
		}

		private async Task<bool> CheckPasswordValid(string password, User user)
		{
			return await UserManager.CheckPasswordAsync(user: user, password: password);
		}

		private async Task<Guid> AddRefreshTokenToDB(Guid userId, string? ipAddress)
		{
			var refreshToken = GenerateRefreshToken(ipAddress);

			refreshToken.UserId = userId;

			await DatabaseContext.UserLogins.AddAsync(refreshToken);

			await DatabaseContext.SaveChangesAsync();

			return refreshToken.RefreshToken;
		}

		private async Task<ClaimsIdentity> CreateClaimsIdentity(User user)
		{
			var userRoles =
				await UserManager.GetRolesAsync(user);

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!),
				new Claim(ClaimTypes.Name, user.UserName!),
				new Claim(ClaimTypes.Email, user.Email!),
				new Claim(nameof(User.SecurityStamp), user.SecurityStamp!),
			};

			foreach (var userRole in userRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var claimIdentity = new ClaimsIdentity(claims);

			return claimIdentity;
		}

		private async Task AddUserLoggedInToCache(Guid userId)
		{
			await Cache.TrySetAsync
				($"userId-{userId}-loggedin", true, TimeSpan.FromHours(1));
		}
		#endregion /Private Methods
	}
}
