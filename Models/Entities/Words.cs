using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class Words : Base.Entity
	{
		[JsonIgnore]
		public Users User { get; set; }
		public string Word { get; set; }
		public bool IsVerb { get; set; }
		public Guid? UserId { get; set; }
		public string Source { get; set; }
		public int WordTypeId { get; set; }
		public int VerbTenseId { get; set; }
		public WordTypes WordType { get; set; }
		public string Description { get; set; }
		public DateTime? EditDate { get; set; }
		public VerbTenses VerbTense { get; set; }
		public DateTime? LearningDate { get; set; }
		public string PersianTranslation { get; set; }
		public string EnglishTranslation { get; set; }
	}
}
