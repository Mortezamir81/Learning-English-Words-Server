using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Services
{
	public interface IWordServices
	{
		Task<Dtat.Results.Result> RemoveWord(string word);

		Task<Dtat.Results.Result> AddNewWord
			(AddWordRequestViewModel addWordRequestViewModel);

		Task<Dtat.Results.Result<List<GetExamResponseViewModel>>>
			GetExam(GetExamRequestViewModel getExamRequestViewModel);

		Task<Dtat.Results.Result> UpdateWord(AddWordRequestViewModel word);

		Task<Dtat.Results.Result<List<GetWordResponseViewModel>>>
			GetAllWords(GetAllWordsRequestViewModel getAllWordsRequestViewModel);

		Task<Dtat.Results.Result<GetWordResponseViewModel>> GetWord(string word);

		Task<Dtat.Results.Result<RecentLearnedResponseViewModel>> GetRecentLearningWords();

		Task<Dtat.Results.Result<ExamProcessingResponseViewModel>>
			ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels);
	}
}
