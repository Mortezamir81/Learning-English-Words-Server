using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Persistence.Repositories
{
	public class WordsRepository : Dtat.Data.EntityFrameworkCore.Repository<Words>, IWordsRepository
	{
		public WordsRepository(DatabaseContext databaseContext) : base(databaseContext)
		{

		}


		//******************** Get Sections ********************
		public async Task<List<string>> GetFourChoiceAnswers
			(string language, QuestionResponse question, Guid userId)
		{
			List<string> fourChoiceAnswers;
			string correctAnswer = null;

			var answers =
				 DbSet
				.Where(current => current.UserId == userId)
				.Where(current => current.PersianTranslation != question.PersianTranslation)
				.Where(current => current.EnglishTranslation != question.EnglishTranslation)
				.OrderBy(current => Guid.NewGuid())
				.Distinct()
				.Take(3);

			switch (language.ToLower())
			{
				case "persian":
					fourChoiceAnswers =
						await answers
						.Select(current => current.PersianTranslation)
						.ToListAsync();

					correctAnswer = question.PersianTranslation;
				break;

				case "english":
					fourChoiceAnswers =
						await answers
						.Select(current => current.EnglishTranslation)
						.ToListAsync();

					correctAnswer = question.EnglishTranslation;
				break;

				default:
					fourChoiceAnswers =
						await answers
						.Select(current => current.EnglishTranslation)
						.ToListAsync();

					correctAnswer = question.EnglishTranslation;
				break;
			}

			if (fourChoiceAnswers == null || fourChoiceAnswers.Count < 3)
			{
				return null;
			}

			fourChoiceAnswers.Add(correctAnswer);

			return fourChoiceAnswers;
		}


		public async Task<List<Words>> GetAllWordAsync
			(GetAllWordsRequestViewModel requestViewModel, Guid userId)
		{
			IQueryable<Words> query = DbSet;

			if (requestViewModel != null)
			{
				//******************** Filter By ********************
				if (!string.IsNullOrWhiteSpace(requestViewModel.StartWith))
				{
					query =
						query.Where(current => current.Word.StartsWith(requestViewModel.StartWith));
				}

				if (!string.IsNullOrWhiteSpace(requestViewModel.EndWith))
				{
					query =
						query.Where(current => current.Word.EndsWith(requestViewModel.EndWith));
				}

				if (!string.IsNullOrWhiteSpace(requestViewModel.Source))
				{
					query =
						query.Where(current => current.Source.Contains(requestViewModel.Source));
				}

				if (!string.IsNullOrWhiteSpace(requestViewModel.PersianTranslation))
				{
					query =
						query.Where(current => current.PersianTranslation.Contains(requestViewModel.PersianTranslation));
				}

				if (!string.IsNullOrWhiteSpace(requestViewModel.EnglishTranslation))
				{
					query =
						query.Where(current => current.EnglishTranslation.Contains(requestViewModel.EnglishTranslation));
				}

				if (requestViewModel.WordTypeId != null)
				{
					query =
						query.Where(current => current.WordTypeId == requestViewModel.WordTypeId);
				}

				if (requestViewModel.VerbTenseId != null)
				{
					query =
						query.Where(current => current.VerbTenseId == requestViewModel.VerbTenseId);
				}

				if (requestViewModel.LearningDate != null)
				{
					query =
						query.Where(current => current.LearningDate == requestViewModel.LearningDate);
				}
				//****************************************


				//******************** Order By ********************
				if (!string.IsNullOrWhiteSpace(requestViewModel.OrderBy))
				{
					if (requestViewModel.OrderBy.ToLower() == "word")
					{
						query =
							query.OrderBy(current => current.Word);
					}

					if (requestViewModel.OrderBy.ToLower() == "datetime")
					{
						query =
							query.OrderBy(current => current.LearningDate);
					}

					if (requestViewModel.OrderBy.ToLower() == "source")
					{
						query =
							query.OrderBy(current => current.Source);
					}

					if (requestViewModel.OrderBy.ToLower() == "wordtypeid")
					{
						query =
							query.OrderBy(current => current.WordTypeId);
					}

					if (requestViewModel.OrderBy.ToLower() == "verbtenseid")
					{
						query =
							query.OrderBy(current => current.VerbTenseId);
					}
				}
				//****************************************
			}

			var result =
				await query
				.AsNoTracking()
				.Where(current => current.UserId == userId)
				.Include(current => current.VerbTense)
				.Include(current => current.WordType)
				.ToListAsync();

			return result;
		}


		public async Task<Guid?> GetWordIdAsync(string word, Guid userId)
		{
			var responseViewModel =
				new RecentLearnedResponseViewModel();

			var result =
				await DbSet
				.AsNoTracking()
				.Where(current => current.Word.ToLower() == word)
				.Where(current => current.UserId == userId)
				.Select(current => current.Id)
				.FirstOrDefaultAsync()
				;

			return result;
		}


		public async Task<Words> GetWordInformationAsync(string word, Guid userId)
		{
			if (string.IsNullOrWhiteSpace(word))
			{
				return null;
			}

			var result =
				await DbSet
					.AsNoTracking()
					.Where(current => current.Word.ToLower() == word.ToLower())
					.Where(current => current.UserId == userId)
					.Include(current => current.WordType)
					.Include(current => current.VerbTense)
					.FirstOrDefaultAsync()
					;

			return result;
		}


		public async Task<RecentLearnedResponseViewModel> GetRecentLearnedWordsAsync(Guid userId)
		{
			var responseViewModel =
				new RecentLearnedResponseViewModel();

			var result =
				await DbSet
				.Where(current => current.UserId == userId)
				.Select(current => new { current.Word, current.LearningDate })
				.OrderByDescending(current => current.LearningDate)
				.Take(10)
				.ToListAsync()
				;

			if (result != null)
			{
				responseViewModel.RecentLearned = new List<string>();

				foreach (var item in result)
				{
					responseViewModel.RecentLearned.Add(item.Word);
				}
			}

			return responseViewModel;
		}
		//****************************************


		public async Task UpdateWordAsync(Words word)
		{
			await Task.Run(() =>
			{
				DatabaseContext.Entry(word).State = EntityState.Modified;
				DatabaseContext.Entry(word).Property(x => x.LearningDate).IsModified = false;
				DatabaseContext.Entry(word).Property(x => x.UserId).IsModified = false;
			});
		}


		public async Task<List<PrimitiveResult>>
			ExamProcessingAsync(List<ExamProcessingRequestViewModel> exams)
		{
			if (exams == null || exams.Count == 0)
				return null;

			var responseViewModels = new List<PrimitiveResult>();

			foreach (var exam in exams)
			{
				if (string.IsNullOrWhiteSpace(exam.Question))
					continue;

				var correctAnswer =
					await DbSet
					.Where(current => current.Word == exam.Question)
					.Select(current => exam.Language == "persian" ? current.PersianTranslation : current.EnglishTranslation)
					.FirstOrDefaultAsync()
					;

				if (string.IsNullOrWhiteSpace(correctAnswer))
					continue;

				responseViewModels.Add(new PrimitiveResult()
				{
					YourAnswer = exam.Answer,
					CorrectAnswer = correctAnswer,
					Question = exam.Question,
					IsCorrect = exam.Answer == correctAnswer ? true : false,
					IsUnanswer = string.IsNullOrWhiteSpace(exam.Answer),
				});
			}

			return responseViewModels;
		}


		public async Task<bool> CheckWordExistAsync(string word, Guid userId)
		{
			if (string.IsNullOrWhiteSpace(word))
				return false;

			var result =
				await DbSet
				.Where(current => current.Word == word)
				.Where(current => current.User.Id == userId)
				.AnyAsync()
				;

			return result;
		}


		public async Task<List<GetExamResponseViewModel>>
			CreateExamAsync(GetExamRequestViewModel requestViewModel, Guid userId)
		{
			IQueryable<Words> query = DbSet;
			List<GetExamResponseViewModel> responseViewModels = null;


			//******************** Optional Options For Create Exam ********************
			if (!string.IsNullOrWhiteSpace(requestViewModel.StartWith))
			{
				query =
					query.Where(current =>
						current.Word.StartsWith(requestViewModel.StartWith));
			}
			if (requestViewModel.LearendDateTime != null)
			{
				query =
					query.Where(current =>
						current.LearningDate == requestViewModel.LearendDateTime);
			}
			//****************************************


			var questions =
				await query
				.Where(current => current.UserId == userId)
				.Select(current => new QuestionResponse()
				{
					Word = current.Word,
					PersianTranslation = current.PersianTranslation,
					EnglishTranslation = current.EnglishTranslation
				})
				.Take(requestViewModel.QuestionsCount ?? 10)
				.ToListAsync();

			if (questions != null && questions.Count > 0)
			{
				responseViewModels = new List<GetExamResponseViewModel>();

				foreach (var question in questions)
				{
					var answers =
						await GetFourChoiceAnswers(language: requestViewModel.LanguageTranslation, question: question, userId: userId);

                    if (answers == null)
                    {
						return null;
					}

					responseViewModels.Add(new GetExamResponseViewModel()
					{
						Question = question,
						Answers = answers
					});
				}
			}

			return responseViewModels;
		}


		public async Task<List<Words>> SearchWordAsync(string searchContent, Guid userId)
		{
            if (string.IsNullOrWhiteSpace(searchContent))
            {
				return null;
            }

			var result =
				await DbSet
				.AsNoTracking()
				.Where(current => current.UserId == userId)
				.Where(current => current.Word.Contains(searchContent))
				.Include(current => current.VerbTense)
				.Include(current => current.WordType)
				.Take(6)
				.ToListAsync()
				;

			return result;
		}
	}
}
