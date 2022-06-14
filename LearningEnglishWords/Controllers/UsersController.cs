namespace Server.Controllers
{
	public class UsersController : BaseApiControllerWithDatabase
	{
		#region Constractor
		public UsersController
			(IUnitOfWork unitOfWork,
			IUserServices userServices,
			Dtat.Logging.ILogger<UsersController> logger) : base(unitOfWork)
		{
			Logger = logger;
			UserServices = userServices;
		}
		#endregion Constractor

		#region Properties
		public IUserServices UserServices { get; }
		public Dtat.Logging.ILogger<UsersController> Logger { get; }
		#endregion /Properties

		#region HttpGet
		[Authorize(UserRoles.Admin)]
		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var serviceResult =
				await UserServices.GetAllUsersAsync();

			return serviceResult.ApiResult();
		}


		[Authorize(UserRoles.All)]
		[HttpGet("GetBasicUserInformation")]
		public async Task<IActionResult> GetUserInformationForUpdate()
		{
			var serviceResult =
				await UserServices.GetUserInformationForUpdate();

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


		[HttpPost("Register")]
		public async Task<IActionResult>
			RegisterAccount(RegisterRequestViewModel requestViewModel)
		{
			var serviceResult =
				await UserServices.RegisterAsync(registerRequestViewModel: requestViewModel);

			return serviceResult.ApiResult();
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("UpdateUserByAdmin")]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult>
			UpdateUserByAdminAsync(UpdateUserByAdminRequestViewModel requestViewModel)
		{
			var serviceResult =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: requestViewModel);

			return serviceResult.ApiResult();
		}


		[HttpDelete("Logout/{refreshToken}")]
		public async Task<IActionResult> LogoutToken(string refreshToken)
		{
			var serviceResult = await
				UserServices.LogoutAsync(refreshToken);

			return serviceResult.ApiResult();
		}


		[Authorize(UserRoles.All)]
		[HttpPut("UpdateProfileImage")]
		public async Task<IActionResult>
			UpdateUserProfileImageAsync([FromForm] IFormFile imageFile, [FromServices] IHostEnvironment HostEnvironment)
		{
			var serviceResult =
				await UserServices.UpdateUserAvatarAsync(file: imageFile, environment: HostEnvironment);

			return serviceResult.ApiResult();
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("ChangeUserRole")]
		[LogInputParameter(InputLogLevel.Warning)]
		public async Task<IActionResult>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel requestViewModel)
		{
			var serviceResult =
				await UserServices.ChangeUserRoleAsync(requestViewModel);

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
		[Authorize(UserRoles.All)]
		[HttpPut]
		public async Task<IActionResult>
			UpdateUserAsync(UpdateUserRequestViewModel updateUserRequestViewModel)
		{
			var serviceResult =
				await UserServices.UpdateUserAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			return serviceResult.ApiResult();
		}
		#endregion /HttpPut

		#region HttpDelete
		[Authorize(UserRoles.All)]
		[HttpDelete("DeleteUserProfileImage")]
		public async Task<IActionResult> 
			DeleteUserProfileImageAsync([FromServices] IHostEnvironment HostEnvironment)
		{
			var serviceResult =
				await UserServices.DeleteUserProfileImageAsync(HostEnvironment: HostEnvironment);

			return serviceResult.ApiResult();
		}
		#endregion /HttpDelete

		#region Methods
		[NonAction]
		private string GetIPAddress()
		{
			var requestHeaders = Request?.Headers;

			if (requestHeaders != null && requestHeaders.ContainsKey("X-Forwarded-For"))
            {
				return Request.Headers["X-Forwarded-For"];
			}
            else
            {
				return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
			}			
		}
		#endregion /Methods
	}
}
