using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Persistence.Repositories
{
	public interface IWordsRepository : Dtat.Data.IRepository<Word>
	{
		Task UpdateWordAsync(Word word);

		Task<Guid?> GetWordIdAsync(string word, Guid userId);

		Task<bool> CheckWordExistAsync(string word, Guid userId);

		Task<Word> GetWordInformationAsync(string word, Guid userId);

		Task<List<Word>> SearchWordAsync(string searchContent, Guid userId);

		Task<RecentLearnedResponseViewModel> GetRecentLearnedWordsAsync(Guid userId);

		Task<List<PrimitiveResult>>
			ExamProcessingAsync(List<ExamProcessingRequestViewModel> examProcessingRequestViewModel);

		Task<List<Word>> GetAllWordAsync(GetAllWordsRequestViewModel getAllWordsRequestViewModel, Guid userId);

		Task<List<GetExamResponseViewModel>> CreateExamAsync(GetExamRequestViewModel getExamRequestViewModel, Guid userId);
	}
}
