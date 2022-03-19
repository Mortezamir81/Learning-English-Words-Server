using ViewModels.Requests;

namespace Infrustructrue.AutoMapperProfiles
{
	public class UserProfile : AutoMapper.Profile
	{
		public UserProfile() : base()
		{
			//For Users && RegisterRequestViewModel
			CreateMap<Domain.Entities.Users, RegisterRequestViewModel>();

			CreateMap<RegisterRequestViewModel, Domain.Entities.Users>()
				.ForMember(dest => dest.Password, opt => opt.MapFrom
					(src => Softmax.Utilities.Security.HashDataBySHA1(src.Password)));

			CreateMap<Domain.Entities.Users, UpdateUserRequestViewModel>();

			CreateMap<UpdateUserRequestViewModel, Domain.Entities.Users>();
		}
	}
}
