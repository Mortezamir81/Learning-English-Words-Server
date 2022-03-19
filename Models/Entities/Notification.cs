using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class Notifications : Base.Entity
	{
		[JsonIgnore]
		public Users User { get; set; }
		public string From { get; set; }
		public bool IsRead { get; set; }
		public Guid UserId { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public bool IsDeleted { get; set; }
		public string Direction { get; set; }
		public DateTime? SentDate { get; set; }
	}
}
