namespace Services
{
	public interface IWordServices
	{
		Task<Result> RemoveWord(string word);


		Task<Result> AddNewWord
			(AddWordRequestViewModel addWordRequestViewModel);


		Task<Result<List<GetExamResponseViewModel>>>
			GetExam(GetExamRequestViewModel getExamRequestViewModel);


		Task<Result> UpdateWord(AddWordRequestViewModel word);


		Task<Result<List<GetWordResponseViewModel>>>
			GetAllWords(GetAllWordsRequestViewModel getAllWordsRequestViewModel);


		Task<Result<GetWordResponseViewModel>> GetWord(string word);


		Task<Result<RecentLearnedResponseViewModel>> GetRecentLearningWords();


		Task<Result<ExamProcessingResponseViewModel>>
			ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels);
	}
}
