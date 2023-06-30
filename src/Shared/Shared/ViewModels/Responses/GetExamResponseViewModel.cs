using System.Collections.Generic;

namespace ViewModels.Responses
{
	public class GetExamResponseViewModel
	{
		public List<string> Answers { get; set; }
		public QuestionResponse Question { get; set; }
	}

	public class QuestionResponse
	{
		public string Word { get; set; }
		public string PersianTranslation{ get; set; }
		public string EnglishTranslation { get; set; }
	}
}
