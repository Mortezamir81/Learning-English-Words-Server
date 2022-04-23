using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class Role
	{
		public Role() : base()
		{
		}


		public int Id {  get; set; }

		public string RoleName {  get; set; }

		[JsonIgnore]
		public IList<User> Users {  get; set; }
	}
}
