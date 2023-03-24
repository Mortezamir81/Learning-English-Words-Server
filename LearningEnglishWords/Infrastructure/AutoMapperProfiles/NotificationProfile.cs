namespace Infrastructure.AutoMapperProfiles
{
	public class NotificationProfile : AutoMapper.Profile
	{
		public NotificationProfile() : base()
		{
			CreateMap<Notifications, GetAllNotificationResponseViewModel>()
				.ForMember(current => current.NotificationId, current => current.MapFrom(current => current.Id));

			CreateMap<GetAllNotificationResponseViewModel, Notifications>()
				.ForMember(current => current.Id, current => current.MapFrom(current => current.NotificationId));

			CreateMap<Notifications, SendNotificationForAllUserRequestViewModel>().ReverseMap();
		}
	}
}
