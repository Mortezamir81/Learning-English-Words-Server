using AutoMapper;
using Persistence;
using Infrustructrue.Utilities;
using Microsoft.AspNetCore.Http;
using Domain.Entities;
using Dtat.Logging;
using Dtat.Results;
using Dtat.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.General;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Services
{
	public partial class WordServices : IWordServices
	{
		#region Constractor
		public WordServices
			(IMapper mapper,
			IUnitOfWork unitOfWork,
			ILogger<WordServices> logger,
			DatabaseContext databaseContext,
			IHttpContextAccessor httpContextAccessor) : base()
		{
			Logger = logger;
			Mapper = mapper;
			UnitOfWork = unitOfWork;
			DatabaseContext = databaseContext;
			HttpContextAccessor = httpContextAccessor;
		}

		#endregion /Constractor

		#region Properties
		public IMapper Mapper { get; }
		public IUnitOfWork UnitOfWork { get; }
		public ILogger<WordServices> Logger { get; }
		public DatabaseContext DatabaseContext { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		#endregion /Properties

		#region Methods
		public async Task<Result> RemoveWord(string word)
		{
			try
			{
				var result = new Result();

				UserInformationInToken user = null;

				if (HttpContextAccessor != null &&
					HttpContextAccessor.HttpContext != null &&
					HttpContextAccessor.HttpContext.Items["User"] != null)
				{
					user =
						HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;
				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var foundedWord =
					await UnitOfWork.WordsRepository.GetWordInformationAsync(word: word, userId: user.Id);

				if (foundedWord == null)
				{
					string wordNotFoundErrorMessage = string.Format
						(Resources.Messages.ErrorMessages.WordNotFound);

					result.AddErrorMessage(wordNotFoundErrorMessage);

					return result;
				}

				await UnitOfWork.WordsRepository.RemoveAsync(foundedWord);
				await UnitOfWork.SaveAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.DeleteWordSuccessful);

				result.AddSuccessMessage(successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical(exception: ex, ex.Message);

				var result =
					new Dtat.Results.Result<RecentLearnedResponseViewModel>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				result.AddErrorMessage(errorMessage);

				return result;
			}
		}


		public async Task<Result<List<GetExamResponseViewModel>>>
			GetExam(GetExamRequestViewModel getExamRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: getExamRequestViewModel);

				await Logger.LogInformation
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result = GetExamValidation(getExamRequestViewModel: getExamRequestViewModel);

				if (result.IsFailed)
					return result;

				UserInformationInToken user = null;

				if (HttpContextAccessor != null &&
					HttpContextAccessor.HttpContext != null &&
					HttpContextAccessor.HttpContext.Items["User"] != null)
				{
					user =
						HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;
				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var respone =
					await UnitOfWork.WordsRepository.CreateExamAsync(getExamRequestViewModel: getExamRequestViewModel, userId: user.Id);

				if (respone == null || respone.Count == 0)
				{
					string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.NotEnoughAnswers);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				result.Value = respone;

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoadExamSuccessfull);

				result.AddSuccessMessage(message: successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical(exception: ex, ex.Message);

				var response =
					new Dtat.Results.Result<List<GetExamResponseViewModel>>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result> UpdateWord(AddWordRequestViewModel word)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: word);

				await Logger.LogInformation
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result =
					UpdateWordValidation(word: word);

				Words responseWord =
					Mapper.Map<Words>(source: word);

				if (result.IsFailed == true)
					return result;

				if (responseWord.WordTypeId == 5)
				{
					responseWord.IsVerb = true;
				}

				if (responseWord.VerbTenseId < 1 || responseWord.VerbTenseId > 17)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidVerbTenseValue);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				if ((responseWord.VerbTenseId != 1) && responseWord.WordTypeId != 5)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidWordTypeStructure);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				if (responseWord.VerbTenseId == 1)
				{
					responseWord.IsVerb = false;
				}

				UserInformationInToken user = null;

				if (HttpContextAccessor != null &&
					HttpContextAccessor.HttpContext != null &&
					HttpContextAccessor.HttpContext.Items["User"] != null)
				{
					user =
						HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;
				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var wordId =
					await UnitOfWork.WordsRepository.GetWordIdAsync(word: responseWord.Word, userId: user.Id);

				if (wordId == null)
				{
					string duplicateErrorMessage = string.Format
						(Resources.Messages.ErrorMessages.WordNotFound);

					result.AddErrorMessage(duplicateErrorMessage);

					return result;
				}

				responseWord.Id = wordId;
				responseWord.EditDate = DateTime.UtcNow;

				await UnitOfWork.WordsRepository.UpdateWordAsync(responseWord);
				await UnitOfWork.SaveAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.UpdateSuccessful);

				result.AddSuccessMessage(message: successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				var response =
					new Dtat.Results.Result();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result<GetWordResponseViewModel>> GetWord(string word)
		{
			Hashtable properties = null;
			try
			{
				properties = new Hashtable();

				properties.Add(key: "word", value: word);

				await Logger.LogInformation
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result = GetWordValidation(word: word);

				if (result.IsFailed == true)
					return result;

				UserInformationInToken user = null;

				if (HttpContextAccessor != null &&
					HttpContextAccessor.HttpContext != null &&
					HttpContextAccessor.HttpContext.Items["User"] != null)
				{
					user =
						HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;
				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var foundedWord =
					await UnitOfWork.WordsRepository.GetWordInformationAsync(word: word, userId: user.Id);

				if (foundedWord == null)
				{
					string wordNotFoundErrorMessage = string.Format
						(Resources.Messages.ErrorMessages.WordNotFound);

					result.AddErrorMessage(wordNotFoundErrorMessage);

					return result;
				}

				GetWordResponseViewModel responseWord =
					Mapper.Map<GetWordResponseViewModel>(source: foundedWord);

				result.Value = responseWord;

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoadWordSuccessfull);

				result.AddSuccessMessage(message: successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				var response =
					new Dtat.Results.Result<GetWordResponseViewModel>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result<List<GetWordResponseViewModel>>>
			GetAllWords(GetAllWordsRequestViewModel getAllWordsRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: getAllWordsRequestViewModel);

				await Logger.LogInformation
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result = new Result<List<GetWordResponseViewModel>>();

				var user =
					HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;

				var foundedWords =
					await UnitOfWork.WordsRepository.GetAllWordAsync(getAllWordsRequestViewModel, user.Id);

				if (foundedWords == null || foundedWords.Count <= 0)
				{
					string wordsListErrorMessage = string.Format
						(Resources.Messages.ErrorMessages.WordsListEmpty);

					result.AddErrorMessage(wordsListErrorMessage);

					return result;
				}

				var wordsList = new List<GetWordResponseViewModel>();

				foreach (var word in foundedWords)
				{
					GetWordResponseViewModel responseWord =
						Mapper.Map<GetWordResponseViewModel>(source: word);

					wordsList.Add(responseWord);
				}

				result.Value = wordsList;

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoadWordSuccessfull);

				result.AddSuccessMessage(message: successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical(exception: ex, ex.Message);

				var response =
					new Dtat.Results.Result<List<GetWordResponseViewModel>>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result<RecentLearnedResponseViewModel>> GetRecentLearningWords()
		{
			try
			{
				var result =
					new Result<RecentLearnedResponseViewModel>();

				UserInformationInToken user = null;

				if (HttpContextAccessor != null &&
					HttpContextAccessor.HttpContext != null &&
					HttpContextAccessor.HttpContext.Items["User"] != null)
				{
					user =
						HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;
				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var recentLearnedWords =
					await UnitOfWork.WordsRepository.GetRecentLearnedWordsAsync(userId: user.Id);

				if (recentLearnedWords == null || recentLearnedWords.RecentLearned == null || recentLearnedWords.RecentLearned.Count == 0)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.WordsListEmpty);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.LoadContentSuccessful);

				result.AddSuccessMessage(successMessage);

				result.Value = new RecentLearnedResponseViewModel();

				result.Value.RecentLearned = new List<string>();

				result.Value.RecentLearned = recentLearnedWords.RecentLearned;

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical(exception: ex, ex.Message);

				var response =
					new Dtat.Results.Result<RecentLearnedResponseViewModel>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result> AddNewWord(AddWordRequestViewModel addWordRequestViewModel)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: addWordRequestViewModel);

				await Logger.LogInformation
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var result =
					AddNewWordValidation(addWordRequestViewModel: addWordRequestViewModel);

				if (result.IsFailed == true)
					return result;

				Words wordModel =
					Mapper.Map<Words>(source: addWordRequestViewModel);

				if (wordModel.WordTypeId == 5)
				{
					wordModel.IsVerb = true;
				}

				if (wordModel.VerbTenseId < 1 || wordModel.VerbTenseId > 17)
				{
					string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidVerbTenseValue);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				if ((wordModel.VerbTenseId != 1) && wordModel.WordTypeId != 5)
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.InvalidWordTypeStructure);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				if (wordModel.VerbTenseId == 1)
				{
					wordModel.IsVerb = false;
				}

				UserInformationInToken user = null;

				if (HttpContextAccessor != null &&
					HttpContextAccessor.HttpContext != null &&
					HttpContextAccessor.HttpContext.Items["User"] != null)
				{
					user =
						HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;
				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				wordModel.UserId = user.Id;

				var isWordExist =
					await UnitOfWork.WordsRepository.CheckWordExistAsync(word: wordModel.Word, userId: user.Id);

				if (isWordExist == true)
				{
					string duplicateErrorMessage = string.Format
						(Resources.Messages.ErrorMessages.DuplicateKey);

					result.AddErrorMessage(duplicateErrorMessage);

					return result;
				}

				await UnitOfWork.WordsRepository.AddAsync(wordModel);
				await UnitOfWork.SaveAsync();

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.AddSuccessful);

				result.AddSuccessMessage(message: successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical
						(exception: ex, ex.Message, parameters: properties);

				var response =
					new Dtat.Results.Result();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}


		public async Task<Result<ExamProcessingResponseViewModel>>
			ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels)
		{
			Hashtable properties = null;
			try
			{
				properties =
					LogUtilities.GetProperties(instance: examProcessingRequestViewModels);

				await Logger.LogInformation
					(message: Resources.Resource.InputPropertiesInfo, parameters: properties);

				var responseValue =
					new ExamProcessingResponseViewModel();

				responseValue.PrimitiveResults = new List<PrimitiveResult>();

				responseValue.CompleteResult = new CompleteResult();

				var result =
					ExamProcessingValidation(examProcessingRequestViewModels);

				if (result.IsFailed)
					return result;

				UserInformationInToken user = null;

				if (HttpContextAccessor != null &&
					HttpContextAccessor.HttpContext != null &&
					HttpContextAccessor.HttpContext.Items["User"] != null)
				{
					user =
						HttpContextAccessor.HttpContext.Items["User"] as UserInformationInToken;
				}
				else
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.UserNotFound);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				var index = 0;
				foreach (var item in examProcessingRequestViewModels)
				{
					var isWordExist =
						await UnitOfWork.WordsRepository.CheckWordExistAsync(item.Question, userId: user.Id);

					if (isWordExist == false)
					{
						string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.WordNotFoundWithIn, $"({item.Question})", nameof(examProcessingRequestViewModels) + $"[{index}]");

						result.AddErrorMessage(errorMessage);
					}

					index++;
				}

				if (result.IsFailed)
					return result;

				var respone =
					await UnitOfWork.WordsRepository.ExamProcessingAsync(examProcessingRequestViewModels);

				if (respone == null || respone.Count == 0)
				{
					string errorMessage = string.Format
							(Resources.Messages.ErrorMessages.UnkonwnError);

					result.AddErrorMessage(errorMessage);

					return result;
				}

				responseValue.PrimitiveResults = respone;

				int correctAnswersCount = 0;
				int incorrectAnswersCount = 0;
				int unanswerCount = 0;

				foreach (var examResult in responseValue.PrimitiveResults)
				{
					if (examResult.IsUnanswer)
					{
						unanswerCount++;
					}
					else
					{
						if (examResult.IsCorrect)
						{
							correctAnswersCount++;
						}
						else
						{
							incorrectAnswersCount++;
						}
					}
				}

				responseValue.CompleteResult.CorrectAnswersCount = correctAnswersCount;
				responseValue.CompleteResult.IncorrectAnswersCount = incorrectAnswersCount;
				responseValue.CompleteResult.UnanswerCount = unanswerCount;
				responseValue.CompleteResult.QuestionsCount = correctAnswersCount + incorrectAnswersCount + unanswerCount;

				Exams exams = new Exams()
				{
					PrimitiveResults = responseValue.PrimitiveResults,
					CompleteResult = responseValue.CompleteResult,
					UserId = user.Id
				};

				await DatabaseContext.Exams.AddAsync(exams);
				await DatabaseContext.SaveChangesAsync();

				result.Value = responseValue;

				string successMessage = string.Format
					(Resources.Messages.SuccessMessages.ExamProcessingSuccessful);

				result.AddSuccessMessage(message: successMessage);

				return result;
			}
			catch (Exception ex)
			{
				await Logger.LogCritical(exception: ex, ex.Message);

				var response =
					new Dtat.Results.Result<ExamProcessingResponseViewModel>();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				response.AddErrorMessage(errorMessage);

				return response;
			}
		}
		#endregion /Methods
	}
}
