using System;
using System.Collections.Generic;

namespace Domain.Entities
{
	public class Users : Base.Entity
	{
		public Users() : base()
		{
		}

		public int RoleId { get; set; }
		public Roles Role { get; set; }
		public string Email { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string Password { get; set; }
		public string Username { get; set; }
		public int PhoneNumber { get; set; }
		public List<Words> Words { get; set; }
		public List<Exams> Exams { get; set; }
		public Guid? SecurityStamp { get; set; }
		public List<Ticket> Tickets { get; set; }
		public DateTime? TimeUpdated { get; set; }
		public DateTime? TimeRegistered { get; set; }
		public List<UserLogins> UserLogins { get; set; }
		public List<Notifications> Notifications { get; set; }
	}
}
