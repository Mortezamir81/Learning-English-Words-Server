using System;

namespace ViewModels.Responses
{
	public class GetAllNotificationResponseViewModel
	{
		public string From { get; set; }
		public bool IsRead { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public string Direction { get; set; }
		public DateTime SentDate { get; set; }
		public Guid NotificationId { get; set; }
	}
}
