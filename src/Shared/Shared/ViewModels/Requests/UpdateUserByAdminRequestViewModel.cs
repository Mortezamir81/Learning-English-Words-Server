namespace ViewModels.Requests
{
	public class UpdateUserByAdminRequestViewModel
	{
		public Guid? Id { get; set; }
		public Guid RoleId { get; set; }
		public string Email { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string UserName { get; set; }
		public int PhoneNumber { get; set; }
	}
}
