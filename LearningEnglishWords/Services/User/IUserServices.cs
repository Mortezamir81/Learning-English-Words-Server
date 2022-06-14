namespace Services
{
	public interface IUserServices
	{
		Task<User> GetByUsernameAsync(string username);


		Task<Result> LogoutAsync(string token);


		Task<Result> RegisterAsync
			(RegisterRequestViewModel registerRequestViewModel);


		Task<Result<List<User>>> GetAllUsersAsync();


		Task<Result> UpdateUserAsync
			(UpdateUserRequestViewModel updateUserRequestViewModel);


		Task<Result> DeleteUserAsync
			(DeleteUserRequestViewModel deleteUserRequestViewModel);


		Task<Result> UpdateUserByAdminAsync
			(UpdateUserByAdminRequestViewModel updateUserRequestViewModel);


		Task<Result>
			DeleteUserProfileImageAsync(IHostEnvironment HostEnvironment);


		Task<Result<UpdateUserProfileResponseViewModel>>
			UpdateUserAvatarAsync(IFormFile file, IHostEnvironment environment);


		Task<Result<LoginResponseViewModel>>
			LoginAsync(LoginRequestViewModel loginRequestViewModel, string ipAddress);


		Task<Result>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel);


		Task<Result<GetUserInformationResponseViewModel>> GetUserInformationForUpdate();


		Task<Result<LoginResponseViewModel>> RefreshTokenAsync(string token, string ipAddress);
	}
}
