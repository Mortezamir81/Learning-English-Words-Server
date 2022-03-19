using System;

namespace ViewModels.General
{
	public class UsersViewModel
	{
		public int RoleId { get; set; }
		public string Email { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string Username { get; set; }
		public int PhoneNumber { get; set; }
		public DateTime? TimeUpdated { get; set; }
		public DateTime? TimeRegistered { get; set; }
	}
}
