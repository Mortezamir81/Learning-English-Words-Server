namespace Services
{
	public interface IWordServices
	{
		Task<Result> RemoveWord(string word, Guid userId);


		Task<Result> AddNewWord
			(AddWordRequestViewModel addWordRequestViewModel, Guid userId);


		Task<Result<List<GetExamResponseViewModel>>>
			GetExam(GetExamRequestViewModel getExamRequestViewModel, Guid userId);


		Task<Result> UpdateWord(AddWordRequestViewModel word, Guid userId);


		Task<Result<List<GetWordResponseViewModel>>>
			GetAllWords(GetAllWordsRequestViewModel getAllWordsRequestViewModel, Guid userId);


		Task<Result<GetWordResponseViewModel>> GetWord(string word, Guid userId);


		Task<Result<RecentLearnedResponseViewModel>> GetRecentLearningWords(Guid userId);


		Task<Result<ExamProcessingResponseViewModel>>
			ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels, Guid userId);
	}
}
