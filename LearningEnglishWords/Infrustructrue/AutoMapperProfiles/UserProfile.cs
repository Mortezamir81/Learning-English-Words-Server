using ViewModels.Requests;

namespace Infrustructrue.AutoMapperProfiles
{
	public class UserProfile : AutoMapper.Profile
	{
		public UserProfile() : base()
		{
			//For Users && RegisterRequestViewModel
			CreateMap<Domain.Entities.Users, RegisterRequestViewModel>();

			CreateMap<RegisterRequestViewModel, Domain.Entities.Users>();

			CreateMap<Domain.Entities.Users, UpdateUserRequestViewModel>();

			CreateMap<UpdateUserRequestViewModel, Domain.Entities.Users>();
		}
	}
}
