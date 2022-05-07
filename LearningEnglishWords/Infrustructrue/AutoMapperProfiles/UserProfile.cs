using ViewModels.Requests;

namespace Infrustructrue.AutoMapperProfiles
{
	public class UserProfile : AutoMapper.Profile
	{
		public UserProfile() : base()
		{
			//For Users && RegisterRequestViewModel
			CreateMap<Domain.Entities.User, RegisterRequestViewModel>();

			CreateMap<RegisterRequestViewModel, Domain.Entities.User>();

			CreateMap<Domain.Entities.User, UpdateUserByAdminRequestViewModel>();

			CreateMap<UpdateUserByAdminRequestViewModel, Domain.Entities.User>();
		}
	}
}
