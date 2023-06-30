namespace Services
{
	public partial class WordServices : IWordServices, IRegisterAsScoped
	{
		#region Constractor
		public WordServices
			(IMapper mapper,
			IUnitOfWork unitOfWork,
			DatabaseContext databaseContext,
			Dtat.Logging.ILogger<WordServices> logger,
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
		public DatabaseContext DatabaseContext { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		public Dtat.Logging.ILogger<WordServices> Logger { get; }
		#endregion /Properties

		#region Methods
		public async Task<Result> RemoveWord(string word, Guid userId)
		{
			var result = new Result();

			var foundedWord =
				await UnitOfWork.WordsRepository.GetWordInformationAsync(word: word, userId: userId);

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


		public async Task<Result<List<GetExamResponseViewModel>>>
			GetExam(GetExamRequestViewModel getExamRequestViewModel, Guid userId)
		{
			var result = GetExamValidation(getExamRequestViewModel: getExamRequestViewModel);

			if (!result.IsSuccess)
				return result;

			var respone =
				await UnitOfWork.WordsRepository.CreateExamAsync
					(getExamRequestViewModel: getExamRequestViewModel, userId: userId);

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


		public async Task<Result> UpdateWord(AddWordRequestViewModel word, Guid userId)
		{
			var result =
				UpdateWordValidation(word: word);

			Word responseWord =
				Mapper.Map<Word>(source: word);

			if (!result.IsSuccess == true)
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

			var wordId =
				await UnitOfWork.WordsRepository.GetWordIdAsync(word: responseWord.Content, userId: userId);

			if (wordId == null)
			{
				string duplicateErrorMessage = string.Format
					(Resources.Messages.ErrorMessages.WordNotFound);

				result.AddErrorMessage(duplicateErrorMessage);

				return result;
			}

			responseWord.Id = wordId.Value;
			responseWord.EditDate = DateTime.UtcNow;

			await UnitOfWork.WordsRepository.UpdateWordAsync(responseWord);
			await UnitOfWork.SaveAsync();

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.UpdateSuccessful);

			result.AddSuccessMessage(message: successMessage);

			return result;
		}


		public async Task<Result<GetWordResponseViewModel>> GetWord(string word, Guid userId)
		{
			var result = GetWordValidation(word: word);

			if (!result.IsSuccess == true)
				return result;

			var foundedWord =
				await UnitOfWork.WordsRepository.GetWordInformationAsync(word: word, userId: userId);

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


		public async Task<Result<List<GetWordResponseViewModel>>>
			GetAllWords(GetAllWordsRequestViewModel getAllWordsRequestViewModel, Guid userId)
		{
			var result = new Result<List<GetWordResponseViewModel>>();

			var foundedWords =
				await UnitOfWork.WordsRepository.GetAllWordAsync(getAllWordsRequestViewModel, userId);

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


		public async Task<Result<RecentLearnedResponseViewModel>> GetRecentLearningWords(Guid userId)
		{
			var result =
				new Result<RecentLearnedResponseViewModel>();

			var recentLearnedWords =
				await UnitOfWork.WordsRepository.GetRecentLearnedWordsAsync(userId: userId);

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


		public async Task<Result> AddNewWord(AddWordRequestViewModel addWordRequestViewModel, Guid userId)
		{
			var result =
				AddNewWordValidation(addWordRequestViewModel: addWordRequestViewModel);

			if (!result.IsSuccess == true)
				return result;

			Word wordModel =
				Mapper.Map<Word>(source: addWordRequestViewModel);

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

			wordModel.UserId = userId;

			var isWordExist =
				await UnitOfWork.WordsRepository.CheckWordExistAsync(word: wordModel.Content, userId: userId);

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


		public async Task<Result<ExamProcessingResponseViewModel>>
			ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels, Guid userId)
		{
			var responseValue =
				new ExamProcessingResponseViewModel();

			responseValue.PrimitiveResults = new List<PrimitiveResult>();

			responseValue.CompleteResult = new CompleteResult();

			var result =
				ExamProcessingValidation(examProcessingRequestViewModels);

			if (!result.IsSuccess)
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

			Exam exams = new Exam()
			{
				PrimitiveResults = responseValue.PrimitiveResults,
				CompleteResult = responseValue.CompleteResult,
				UserId = userId
			};

			await DatabaseContext.Exams.AddAsync(exams);
			await DatabaseContext.SaveChangesAsync();

			result.Value = responseValue;

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.ExamProcessingSuccessful);

			result.AddSuccessMessage(message: successMessage);

			return result;
		}
		#endregion /Methods
	}
}
