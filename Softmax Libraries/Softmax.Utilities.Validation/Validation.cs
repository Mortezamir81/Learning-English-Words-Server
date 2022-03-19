using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Dtat.Utilities
{
	public class Validation
	{
		public static bool CheckEmailValid(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return false;

			if (email.Trim().EndsWith("."))
			{
				return false; // suggested by @TK-421
			}
			try
			{
				var addr = new MailAddress(email.Trim());
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
