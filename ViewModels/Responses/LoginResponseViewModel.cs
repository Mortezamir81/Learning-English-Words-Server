using System;


namespace ViewModels.Responses
{
	public class LoginResponseViewModel
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public string Username { get; set; }
		public Guid RefreshToken { get; set; }
	}
}
