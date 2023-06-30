namespace Server.Controllers.V1
{
	public class UsersController : BaseApiControllerWithDatabase
	{
		#region Constractor
		public UsersController
			(IUnitOfWork unitOfWork,
			IUserServices userServices,
			ILogger<UsersController> logger) : base(unitOfWork)
		{
			Logger = logger;
			UserServices = userServices;
		}
		#endregion Constractor

		#region Properties
		public IUserServices UserServices { get; }
		public ILogger<UsersController> Logger { get; }
		#endregion /Properties

		#region HttpGet
		[Authorize(Roles = $"{Constants.Role.SystemAdmin}")]
		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var serviceResult =
				await UserServices.GetAllUsersAsync();

			return serviceResult.ApiResult();
		}


		[Authorize]
		[HttpGet("GetBasicUserInformation")]
		public async Task<IActionResult> GetUserInformationForUpdate()
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await UserServices.GetUserInformationForUpdate(userId: userId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpGet

		#region HttpPost
		[HttpPost("Login")]
		public async Task<IActionResult>
			LoginAsync(LoginRequestViewModel requestViewModel)
		{
			var serviceResult =
				await UserServices.LoginAsync(requestViewModel, ipAddress: GetIPAddress());

			return serviceResult.ApiResult();
		}


		/// <summary>
		/// Login user with username and password by OAuth2
		/// </summary>
		[HttpPost("LoginByOAuth")]
		public async Task<ActionResult<Result<LoginByOAuthResponseViewModel>>>
			LoginByOAuthAsync([FromForm] LoginByOAuthRequestViewModel requestViewModel)
		{
			var serviceResult =
				await UserServices.LoginByOAuthAsync(requestViewModel, ipAddress: GetIPAddress());

			if (!serviceResult.IsSuccess)
			{
				return BadRequest();
			}

			return Ok(serviceResult.Value);
		}


		[HttpPost("Register")]
		public async Task<IActionResult>
			RegisterAccount(RegisterRequestViewModel requestViewModel)
		{
			var serviceResult =
				await UserServices.RegisterAsync(registerRequestViewModel: requestViewModel);

			return serviceResult.ApiResult();
		}


		[HttpPost("RefreshToken/{refreshToken?}")]
		public async Task<IActionResult> RefreshToken(string refreshToken)
		{
			var serviceResult =
				await UserServices.RefreshTokenAsync(token: refreshToken, ipAddress: GetIPAddress());

			return serviceResult.ApiResult();
		}
		#endregion /HttpPost

		#region HttpPut
		[Authorize]
		[HttpPut]
		public async Task<IActionResult>
			UpdateUserAsync(UpdateUserRequestViewModel updateUserRequestViewModel)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await UserServices.UpdateUserAsync(updateUserRequestViewModel: updateUserRequestViewModel, userId: userId);

			return serviceResult.ApiResult();
		}


		[Authorize(Roles = $"{Constants.Role.SystemAdmin}, {Constants.Role.Admin}")]
		[HttpPut("UpdateUserByAdmin")]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult>
			UpdateUserByAdminAsync(UpdateUserByAdminRequestViewModel requestViewModel)
		{
			var serviceResult =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: requestViewModel);

			return serviceResult.ApiResult();
		}


		[Authorize(Roles = $"{Constants.Role.SystemAdmin}, {Constants.Role.Admin}")]
		[HttpPut("ChangeUserRole")]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel requestViewModel)
		{
			var adminId = GetRequierdUserId();

			var serviceResult =
				await UserServices.ChangeUserRoleAsync(requestViewModel, adminId);

			return serviceResult.ApiResult();
		}


		[Authorize]
		[HttpPut("UpdateProfileImage")]
		public async Task<IActionResult>
			UpdateUserAvatarImageAsync([FromForm] IFormFile imageFile, [FromServices] IHostEnvironment HostEnvironment)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await UserServices.UpdateUserAvatarAsync(file: imageFile, environment: HostEnvironment, userId: userId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpPut

		#region HttpDelete
		[Authorize]
		[HttpDelete("DeleteUserProfileImage")]
		public async Task<IActionResult>
			DeleteUserProfileImageAsync([FromServices] IHostEnvironment HostEnvironment)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await UserServices.DeleteUserProfileImageAsync(HostEnvironment: HostEnvironment, userId: userId);

			return serviceResult.ApiResult();
		}


		[HttpDelete("Logout/{refreshToken}")]
		public async Task<IActionResult> LogoutToken(string refreshToken)
		{
			var serviceResult = await
				UserServices.LogoutAsync(refreshToken);

			return serviceResult.ApiResult();
		}
		#endregion /HttpDelete

		#region Methods
		[NonAction]
		private string? GetIPAddress()
		{
			var requestHeaders = Request?.Headers;

			if (requestHeaders != null && requestHeaders.ContainsKey("X-Forwarded-For"))
			{
				return Request?.Headers["X-Forwarded-For"];
			}
			else
			{
				return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
			}
		}
		#endregion /Methods
	}
}
