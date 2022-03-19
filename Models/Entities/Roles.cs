using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class Roles
	{
		public int Id {  get; set; }

		public string RoleName {  get; set; }

		[JsonIgnore]
		public IList<Users> Users {  get; set; }
	}
}
