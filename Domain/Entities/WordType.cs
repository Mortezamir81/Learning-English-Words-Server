using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class WordType
	{
		public WordType() : base()
		{
		}


		public int Id { get; set; }
		public string Type { get; set; }

		[JsonIgnore]
		public IList<Word> Words { get; set; }
	}
}
