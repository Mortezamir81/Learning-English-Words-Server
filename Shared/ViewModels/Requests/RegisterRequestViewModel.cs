namespace ViewModels.Requests
{
	public class RegisterRequestViewModel
	{
		public RegisterRequestViewModel() : base()
		{
		}

		public string Email { get; set; }
		public string Password { get; set; }
		public string UserName { get; set; }
	}
}
