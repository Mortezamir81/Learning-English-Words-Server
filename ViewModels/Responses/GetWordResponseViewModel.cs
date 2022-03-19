using System;

namespace ViewModels.Responses
{
	public class GetWordResponseViewModel
	{
		public string Word { get; set; }
		public bool IsVerb { get; set; }
		public string Source { get; set; }
		public int WordTypeId { get; set; }
		public string WordType { get; set; }
		public int VerbTenseId { get; set; }
		public string VerbTense { get; set; }
		public string Description { get; set; }
		public DateTime? EditDate { get; set; }
		public DateTime? LearningDate { get; set; }
		public string PersianTranslation { get; set; }
		public string EnglishTranslation { get; set; }
	}
}
