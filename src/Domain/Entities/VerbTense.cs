using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class VerbTense
	{
		public VerbTense() : base()
		{
		}


		public int Id { get; set; }
		public string Tense { get; set; }

		[JsonIgnore]
		public IList<Word> Words { get; set; }
	}
}
