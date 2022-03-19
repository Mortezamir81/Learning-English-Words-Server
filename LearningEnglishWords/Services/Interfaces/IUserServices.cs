using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Services
{
	public interface IUserServices
	{
		Task<Users> GetByUsernameAsync(string username);

		Task<Dtat.Results.Result> LogoutAsync(string token);

		Task<Dtat.Results.Result> RegisterAsync
			(RegisterRequestViewModel registerRequestViewModel);

		Task<Dtat.Results.Result> DeleteUsersAsync
			(DeleteUserRequestViewModel deleteUserRequestViewModel);

		Task<Dtat.Results.Result> UpdateUserAsync
			(UpdateUserRequestViewModel updateUserRequestViewModel);

		Task<Dtat.Results.Result<List<Users>>> GetAllUsersAsync();

		Task<Dtat.Results.Result>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel);

		Task<Dtat.Results.Result<LoginResponseViewModel>>
			LoginAsync(LoginRequestViewModel loginRequestViewModel, string ipAddress);

		Task<Dtat.Results.Result<LoginResponseViewModel>> RefreshTokenAsync(string token, string ipAddress);
	}
}
