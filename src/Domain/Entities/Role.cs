using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
	public class Role : IdentityRole<Guid>
	{
		public Role(string name)
		{
			Name = name;
		}
	}
}
