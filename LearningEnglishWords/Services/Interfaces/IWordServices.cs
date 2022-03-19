using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Services
{
	public interface IWordServices
	{
		Task<Softmax.Results.Result> RemoveWord(string word);

		Task<Softmax.Results.Result> AddNewWord
			(AddWordRequestViewModel addWordRequestViewModel);

		Task<Softmax.Results.Result<List<GetExamResponseViewModel>>>
			GetExam(GetExamRequestViewModel getExamRequestViewModel);

		Task<Softmax.Results.Result> UpdateWord(AddWordRequestViewModel word);

		Task<Softmax.Results.Result<List<GetWordResponseViewModel>>>
			GetAllWords(GetAllWordsRequestViewModel getAllWordsRequestViewModel);

		Task<Softmax.Results.Result<GetWordResponseViewModel>> GetWord(string word);

		Task<Softmax.Results.Result<RecentLearnedResponseViewModel>> GetRecentLearningWords();

		Task<Softmax.Results.Result<ExamProcessingResponseViewModel>>
			ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels);
	}
}
