using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class VerbTenses
	{
		public int Id { get; set; }
		public string Tense { get; set; }

		[JsonIgnore]
		public IList<Words> Words { get; set; }
	}
}
