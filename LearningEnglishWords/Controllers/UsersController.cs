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
			var result =
				await UserServices.GetAllUsersAsync();

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[Authorize(UserRoles.All)]
		[HttpGet("GetBasicUserInformation")]
		public async Task<IActionResult> GetUserInformationForUpdate()
		{
			var result =
				await UserServices.GetUserInformationForUpdate();

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}
		#endregion /HttpGet

		#region HttpPost
		[HttpPost("Login")]
		public async Task<IActionResult>
			LoginAsync([FromBody] LoginRequestViewModel viewModel)
		{
			var response =
				await UserServices.LoginAsync(viewModel, ipAddress: GetIPAddress());

			if (response.IsFailed)
				return BadRequest(response);

			return Ok(response);
		}


		[HttpPost("Register")]
		public async Task<IActionResult>
			RegisterAccount([FromBody] RegisterRequestViewModel registerRequestViewModel)
		{
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("UpdateUserByAdmin")]
		public async Task<IActionResult>
			UpdateUserByAdminAsync([FromBody] UpdateUserByAdminRequestViewModel updateUserByAdminRequestViewModel)
		{
			var result =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserByAdminRequestViewModel);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[HttpDelete("Logout/{refreshToken}")]
		public async Task<IActionResult> LogoutToken(string refreshToken)
		{
			var response = await
				UserServices.LogoutAsync(refreshToken);

			if (response.IsFailed)
				return BadRequest(response);

			return Ok(response);
		}


		[Authorize(UserRoles.All)]
		[HttpPut("UpdateProfileImage")]
		public async Task<IActionResult>
			UpdateUserProfileImageAsync(IFormFile file, [FromServices] IHostEnvironment HostEnvironment)
		{
			var result =
				await UserServices.UpdateUserProfileAsync(file: file, environment: HostEnvironment);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("ChangeUserRole")]
		public async Task<IActionResult>
			ChangeUserRoleAsync([FromBody] ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel)
		{
			var result =
				await UserServices.ChangeUserRoleAsync(changeUserRoleRequestViewModel);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[HttpPost("RefreshToken/{refreshToken?}")]
		public async Task<IActionResult> RefreshToken(string refreshToken)
		{
			var response =
				await UserServices.RefreshTokenAsync(token: refreshToken, ipAddress: GetIPAddress());

			if (response.IsFailed)
				return BadRequest(response);

			return Ok(response);
		}
		#endregion /HttpPost

		#region HttpPut
		[Authorize(UserRoles.All)]
		[HttpPut]
		public async Task<IActionResult>
			UpdateUserAsync(UpdateUserRequestViewModel updateUserRequestViewModel)
		{
			var result =
				await UserServices.UpdateUserAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}
		#endregion /HttpPut

		#region HttpDelete
		[Authorize(UserRoles.All)]
		[HttpDelete("DeleteUserProfileImage")]
		public async Task<IActionResult> 
			DeleteUserProfileImageAsync([FromServices] IHostEnvironment HostEnvironment)
		{
			var result =
				await UserServices.DeleteUserProfileImageAsync(HostEnvironment: HostEnvironment);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
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
