using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class UserLogin : Base.Entity
	{
		public UserLogin() : base()
		{
		}


		[JsonIgnore]
		public User User { get; set; }
		public Guid UserId { get; set; }
		public DateTime Expires { get; set; }
		public DateTime Created { get; set; }
		public Guid RefreshToken { get; set; }
		public string CreatedByIp { get; set; }
		public bool IsExpired
		{
			get
			{
				return DateTime.UtcNow > Expires;
			}
		}
	}
}
