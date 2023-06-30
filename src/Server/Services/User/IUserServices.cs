namespace Services
{
	public interface IUserServices
	{
		Task<User> GetByUserNameAsync(string userName);


		Task<Result> LogoutAsync(string token);


		Task<Result> RegisterAsync
			(RegisterRequestViewModel registerRequestViewModel);


		Task<Result<List<User>>> GetAllUsersAsync();


		Task<Result> UpdateUserAsync
			(UpdateUserRequestViewModel updateUserRequestViewModel, Guid userId);


		Task<Result> DeleteUserAsync
			(DeleteUserRequestViewModel deleteUserRequestViewModel);


		Task<Result> UpdateUserByAdminAsync
			(UpdateUserByAdminRequestViewModel updateUserRequestViewModel);


		Task<Result>
			DeleteUserProfileImageAsync(IHostEnvironment HostEnvironment, Guid userId);


		Task<Result<UpdateUserProfileResponseViewModel>>
			UpdateUserAvatarAsync(IFormFile file, IHostEnvironment environment, Guid userId);


		Task<Result<LoginResponseViewModel>>
			LoginAsync(LoginRequestViewModel loginRequestViewModel, string ipAddress);


		Task<Result<LoginByOAuthResponseViewModel>>
			LoginByOAuthAsync(LoginByOAuthRequestViewModel requestViewModel, string? ipAddress);


		Task<Result>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel, Guid adminId);


		Task<Result<GetUserInformationResponseViewModel>> GetUserInformationForUpdate(Guid userId);


		Task<Result<LoginResponseViewModel>> RefreshTokenAsync(string token, string ipAddress);
	}
}
