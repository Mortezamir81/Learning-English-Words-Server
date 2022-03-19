using System;

namespace ViewModels.Requests
{
	public class GetExamRequestViewModel
	{
		public string StartWith { get; set; }
		public int? QuestionsCount { get; set; }
		public DateTime? LearendDateTime { get; set; }
		public string LanguageTranslation { get; set; }
	}
}
