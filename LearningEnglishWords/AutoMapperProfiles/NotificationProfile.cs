using ViewModels.Requests;
using ViewModels.Responses;

namespace Infrustructrue.AutoMapperProfiles
{
	public class NotificationProfile : AutoMapper.Profile
	{
		public NotificationProfile() : base()
		{
			CreateMap<Domain.Entities.Notifications, GetAllNotificationResponseViewModel>()
				.ForMember(current => current.NotificationId, current => current.MapFrom(current => current.Id));

			CreateMap<GetAllNotificationResponseViewModel, Domain.Entities.Notifications>();

			CreateMap<Domain.Entities.Notifications, SendNotificationForAllUserRequestViewModel>();

			CreateMap<SendNotificationForAllUserRequestViewModel, Domain.Entities.Notifications>();
		}
	}
}
