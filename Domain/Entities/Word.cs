using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
	public class Word : Base.Entity
	{
		public Word() : base()
		{
		}


		[JsonIgnore]
		public User User { get; set; }
		public string Content { get; set; }
		public bool IsVerb { get; set; }
		public Guid? UserId { get; set; }
		public string Source { get; set; }
		public int WordTypeId { get; set; }
		public int VerbTenseId { get; set; }
		public WordType WordType { get; set; }
		public string Description { get; set; }
		public DateTime? EditDate { get; set; }
		public VerbTense VerbTense { get; set; }
		public DateTime? LearningDate { get; set; }
		public string PersianTranslation { get; set; }
		public string EnglishTranslation { get; set; }
	}
}
