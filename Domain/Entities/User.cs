using Domain.SeedWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
	public class User : IdentityUser<Guid>, IEntityHasIsSystemic
	{
		public User(string userName) : base()
		{
			UserName = userName;
		}

		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string Password { get; set; }
		public List<Word> Words { get; set; }
		public List<Exam> Exams { get; set; }
		public string ProfileImage { get; set; }
		public List<Ticket> Tickets { get; set; }
		public DateTime? TimeUpdated { get; set; }
		public DateTime? TimeRegistered { get; set; }
		public List<UserLogin> UserLogins { get; set; }
		public List<Notifications> Notifications { get; set; }
		public bool IsSystemic { get; set; }
		public bool IsBanned { get; set; }
	}
}
