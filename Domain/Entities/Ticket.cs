using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class Ticket : Base.Entity
	{
		public Ticket() : base()
		{
		}


		[JsonIgnore]
		public Users User { get; set; }
		public Guid UserId { get; set; }
		public string Method { get; set; }
		public string Message { get; set; }
		public DateTime? SentDate { get; set; }
	}
}
