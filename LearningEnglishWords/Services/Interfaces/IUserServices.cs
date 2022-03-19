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

		Task<Softmax.Results.Result> LogoutAsync(string token);

		Task<Softmax.Results.Result> RegisterAsync
			(RegisterRequestViewModel registerRequestViewModel);

		Task<Softmax.Results.Result> DeleteUsersAsync
			(DeleteUserRequestViewModel deleteUserRequestViewModel);

		Task<Softmax.Results.Result> UpdateUserAsync
			(UpdateUserRequestViewModel updateUserRequestViewModel);

		Task<Softmax.Results.Result<List<Users>>> GetAllUsersAsync();

		Task<Softmax.Results.Result>
			ChangeUserRoleAsync(ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel);

		Task<Softmax.Results.Result<LoginResponseViewModel>>
			LoginAsync(LoginRequestViewModel loginRequestViewModel, string ipAddress);

		Task<Softmax.Results.Result<LoginResponseViewModel>> RefreshTokenAsync(string token, string ipAddress);
	}
}
