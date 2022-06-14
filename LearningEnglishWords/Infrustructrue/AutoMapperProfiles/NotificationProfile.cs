namespace Infrustructrue.AutoMapperProfiles
{
	public class NotificationProfile : AutoMapper.Profile
	{
		public NotificationProfile() : base()
		{
			CreateMap<Notifications, GetAllNotificationResponseViewModel>()
				.ForMember(current => current.NotificationId, current => current.MapFrom(current => current.Id));

			CreateMap<GetAllNotificationResponseViewModel, Notifications>().ReverseMap();

			CreateMap<Notifications, SendNotificationForAllUserRequestViewModel>().ReverseMap();
		}
	}
}
