using System;

namespace ViewModels.Requests
{
	public class GetAllWordsRequestViewModel
	{
		public string Source { get; set; }
		public string OrderBy { get; set; }
		public string EndWith { get; set; }
		public int? WordTypeId { get; set; }
		public int? VerbTenseId { get; set; }
		public string StartWith { get; set; }
		public DateTime? LearningDate { get; set; }
		public string EnglishTranslation { get; set; }
		public string PersianTranslation { get; set; }
	}
}
