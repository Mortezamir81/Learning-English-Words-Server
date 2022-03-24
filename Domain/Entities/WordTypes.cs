using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class WordTypes
	{
		public WordTypes() : base()
		{
		}


		public int Id { get; set; }
		public string Type { get; set; }

		[JsonIgnore]
		public IList<Words> Words { get; set; }
	}
}
