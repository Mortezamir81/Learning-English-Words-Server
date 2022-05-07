using System;
using System.Collections.Generic;

namespace Domain.Entities
{
	public class User : Base.Entity
	{
		public User() : base()
		{
		}

		public int RoleId { get; set; }
		public Role Role { get; set; }
		public string Email { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string Password { get; set; }
		public string Username { get; set; }
		public string PhoneNumber { get; set; }
		public List<Word> Words { get; set; }
		public List<Exam> Exams { get; set; }
		public Guid? SecurityStamp { get; set; }
		public string ProfileImage { get; set; }
		public List<Ticket> Tickets { get; set; }
		public DateTime? TimeUpdated { get; set; }
		public DateTime? TimeRegistered { get; set; }
		public List<UserLogin> UserLogins { get; set; }
		public List<Notifications> Notifications { get; set; }

	}
}
