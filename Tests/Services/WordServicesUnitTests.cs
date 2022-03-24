using AutoMapper;
using Persistence;
using Services;
using ViewModels.Requests;
using Microsoft.EntityFrameworkCore;
using Dtat.Logging;
using Dtat.Logging.NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace MaxLearnTest.Services
{
	public class WordServicesUnitTests
	{
		public WordServices WordServices { get; set; }
		UnitOfWork unitOfWork;
		public WordServicesUnitTests()
		{
			DbContextOptionsBuilder options = new DbContextOptionsBuilder();
			options.UseSqlServer(connectionString: "Data Source = SQL5107.site4now.net;  Initial Catalog = db_a842d3_learningenglish; User ID = db_a842d3_learningenglish_admin; Password = m@13811386;");

			DatabaseContext databaseContext = new DatabaseContext(options.Options);

			unitOfWork = new UnitOfWork(databaseContext);

			ILogger<WordServices> loggerForWordServices = new NLogAdapter<WordServices>(null);

			var config = new MapperConfiguration(cfg => {
				cfg.AddProfile<Infrustructrue.AutoMapperProfiles.WordProfile>();
			});

			Mapper mapper = new Mapper(config);

			HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            WordServices =
                new WordServices
                    (unitOfWork: unitOfWork,
                    logger: loggerForWordServices,
                    mapper: mapper,
                    httpContextAccessor: httpContextAccessor,
					databaseContext: databaseContext);
        }

        #region AddWord
        [Fact]
		public async Task TestAddWordWithNullWord()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(addWordRequestViewModel.Word), nameof(addWordRequestViewModel));

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestAddWordWithNullSource()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = "test",
				Source = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(addWordRequestViewModel.Source), nameof(addWordRequestViewModel));

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestAddWordWithNullEnglishTranslation()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = "test",
				Source = "test",
				PersianTranslation = "test",
				WordTypeId = 1,
				EnglishTranslation = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(addWordRequestViewModel.EnglishTranslation), nameof(addWordRequestViewModel));

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestAddWordWithNullPersianTranslation()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = "test",
				Source = "test",
				WordTypeId = 1,
				EnglishTranslation = "test",
				PersianTranslation = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(addWordRequestViewModel.PersianTranslation), nameof(addWordRequestViewModel));

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestAddWordWithInvalidWordTypeId()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = "test",
				Source = "test",
				PersianTranslation = "test",
				EnglishTranslation = "test",
				WordTypeId = 7
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidWordTypeValue);

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestAddWordWithInvalidVerbTenseId()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = "test",
				Source = "test",
				PersianTranslation = "test",
				EnglishTranslation = "test",
				WordTypeId = 6,
				VerbTenseId = -1,
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidVerbTenseValue);

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestAddWordWithPassingVerbTenseWithoutWordTypeVerb()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = "test",
				Source = "test",
				PersianTranslation = "test",
				EnglishTranslation = "test",
				WordTypeId = 6,
				VerbTenseId = 2
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidWordTypeStructure);

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestAddWordWithNullValues()
		{
			//Arrange
			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = null,
				Source = null,
				PersianTranslation = null,
				EnglishTranslation = null,
				WordTypeId = 7,
			};

			string wordErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.Word), nameof(addWordRequestViewModel));


			string sourceErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.Source), nameof(addWordRequestViewModel));

			string persianTranslationErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.PersianTranslation), nameof(addWordRequestViewModel));

			string englishTranslationErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.EnglishTranslation), nameof(addWordRequestViewModel));

			string wordTypeIdErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidWordTypeValue);

			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			var wordErrorResult =
				result.Errors.Where(current => current == wordErrorMessage).SingleOrDefault();

			var sourceErrorResult =
				result.Errors.Where(current => current == sourceErrorMessage).SingleOrDefault();


			var persianTranslationErrorResult =
				result.Errors.Where(current => current == persianTranslationErrorMessage).SingleOrDefault();


			var englishTranslationErrorResult =
				result.Errors.Where(current => current == englishTranslationErrorMessage).SingleOrDefault();

			var wordTypeIdErrorResult =
				result.Errors.Where(current => current == wordTypeIdErrorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: wordErrorMessage, actual: wordErrorResult);
			Assert.Equal(expected: sourceErrorResult, actual: sourceErrorResult);
			Assert.Equal(expected: persianTranslationErrorMessage, actual: persianTranslationErrorResult);
			Assert.Equal(expected: englishTranslationErrorMessage, actual: englishTranslationErrorResult);
			Assert.Equal(expected: wordTypeIdErrorMessage, actual: wordTypeIdErrorResult);
		}

		[Fact]
		public async Task TestAddWordWithCorrectValues()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = word,
				Source = "test",
				PersianTranslation = "test",
				EnglishTranslation = "test",
				WordTypeId = 5,
				VerbTenseId = 2
			};

			var users =
				await unitOfWork.UserRepository.GetAllAsync();

			var user = users.FirstOrDefault();

			WordServices.HttpContextAccessor.HttpContext = new DefaultHttpContext();

			WordServices.HttpContextAccessor.HttpContext.Items["User"] = user;

			//Act
			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			var allWords =
				await unitOfWork.WordsRepository.GetAllAsync();

			var findedWord =
				allWords.Where(current => current.Word == word);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.AddSuccessful);

			//Assert
			Assert.Equal(expected: successMessage, actual: result.Successes[0]);
			Assert.NotNull(findedWord);
		}
		#endregion /AddWord

		#region GetWord
		[Fact]
		public async Task TestGetWordWithNullWord()
		{
			//Arrange
			string word = null;

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(word), nameof(word));

			//Act
			var result =
				await WordServices.GetWord(word: word);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestGetWordWithInvalidWord()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.WordNotFound);

			//Act
			var result =
				await WordServices.GetWord(word: word);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestGetWordWithValidWord()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = word,
				Source = "test",
				PersianTranslation = "test",
				EnglishTranslation = "test",
				WordTypeId = 1,
				VerbTenseId = 1,
			};

			var users =
				await unitOfWork.UserRepository.GetAllAsync();

			var user = users.FirstOrDefault();

			WordServices.HttpContextAccessor.HttpContext = new DefaultHttpContext();

			WordServices.HttpContextAccessor.HttpContext.Items["User"] = user;

			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Act
			var result2 =
				await WordServices.GetWord(word: word);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoadWordSuccessfull);

			//Assert
			Assert.Equal(expected: successMessage, actual: result2.Successes[0]);
			Assert.NotNull(result2.Value);
			Assert.NotNull(result2.Value.WordType);
			Assert.NotNull(result2.Value.VerbTense);
		}
		#endregion /GetWord

		#region GetAllWord
		[Fact]
		public async Task TestGetAllWordWithEmptyWordsList()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.WordsListEmpty);

			var allWords =
				await unitOfWork.WordsRepository.GetAllAsync();

			foreach (var word1 in allWords)
			{
				await unitOfWork.WordsRepository.RemoveAsync(word1);
				await unitOfWork.SaveAsync();
			}

			//Act
			var result =
				await WordServices.GetAllWords(null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
			Assert.Null(result.Value);
		}

		[Fact]
		public async Task TestGetAllWordWithFillterinAndOrderBy()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = "a" + word,
				Source = "test",
				PersianTranslation = "test",
				EnglishTranslation = "test",
				WordTypeId = 1,
				VerbTenseId = 1,
			};

			var users =
				await unitOfWork.UserRepository.GetAllAsync();

			var user = users.FirstOrDefault();

			WordServices.HttpContextAccessor.HttpContext = new DefaultHttpContext();

			WordServices.HttpContextAccessor.HttpContext.Items["User"] = user;

			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			addWordRequestViewModel.Word = "b" + word;

			var result2 =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Act
			var result3 =
				await WordServices.GetAllWords(new GetAllWordsRequestViewModel()
				{
					EndWith = word,
					VerbTenseId = 1,
					WordTypeId = 1,
					Source = "test",
					PersianTranslation = "test",
					OrderBy = "word",
				});

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoadWordSuccessfull);

			//Assert
			Assert.Equal(expected: successMessage, actual: result3.Successes[0]);
			Assert.NotNull(result3.Value);
			Assert.NotNull(result3.Value[0].WordType);
			Assert.NotNull(result3.Value[0].VerbTense);
			Assert.Equal(actual: result3.Value[0].Word, expected: "a" + word);
			Assert.Equal(actual: result3.Value[1].Word, expected: "b" + word);
		}

		[Fact]
		public async Task TestGetAllWord()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = word,
				Source = "test",
				PersianTranslation = "test",
				EnglishTranslation = "test",
				WordTypeId = 1,
				VerbTenseId = 1,
			};

			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			//Act
			var result2 =
				await WordServices.GetAllWords(null);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoadWordSuccessfull);

			//Assert
			Assert.Equal(expected: successMessage, actual: result2.Successes[0]);
			Assert.NotNull(result2.Value);
			Assert.NotNull(result2.Value[0].WordType);
			Assert.NotNull(result2.Value[0].VerbTense);
		}
		#endregion /GetAllWord

		#region GetExam
		[Fact]
		public async Task TestGetExamWithInvalidQuestionCount()
		{
			//Arrange
			var getExamRequestViewModel = new GetExamRequestViewModel()
			{
				QuestionsCount = -1
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidQuestionCountValue);

			//Act
			var result =
				await WordServices.GetExam(getExamRequestViewModel: getExamRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
			Assert.Null(result.Value);
		}

		[Fact]
		public async Task TestGetExam()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = word + "a",
				Source = "test",
				PersianTranslation = "english",
				EnglishTranslation = "فارسی",
				WordTypeId = 1,
				VerbTenseId = 1,
			};

			var users =
				await unitOfWork.UserRepository.GetAllAsync();

			var user = users.FirstOrDefault();

			WordServices.HttpContextAccessor.HttpContext = new DefaultHttpContext();

			WordServices.HttpContextAccessor.HttpContext.Items["User"] = user;

			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			AddWordRequestViewModel addWordRequestViewModel2 = new AddWordRequestViewModel()
			{
				Word = word + "b",
				Source = "test",
				PersianTranslation = "english",
				EnglishTranslation = "فارسی",
				WordTypeId = 1,
				VerbTenseId = 1,
			};

			var result2 =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel2);


			var getExamRequestViewModel = new GetExamRequestViewModel()
			{
				QuestionsCount = 2,
				StartWith = word,
				LanguageTranslation = "english"
			};


			//Act
			var result3 =
				await WordServices.GetExam(getExamRequestViewModel: getExamRequestViewModel);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoadExamSuccessfull);

			//Assert
			Assert.Equal(expected: successMessage, actual: result3.Successes[0]);
			Assert.NotNull(result3.Value);
			Assert.NotNull(result3.Value[0].Question);
			Assert.NotNull(result3.Value[0].Answers);
			Assert.NotNull(result3.Value[1].Question);
			Assert.NotNull(result3.Value[1].Answers);
		}
		#endregion /GetExam

		#region ExamProcessing
		[Fact]
		public async Task TestExamProcessing()
		{
			//Arrange
			string word = Guid.NewGuid().ToString();

			AddWordRequestViewModel addWordRequestViewModel = new AddWordRequestViewModel()
			{
				Word = word + "a",
				Source = "test",
				EnglishTranslation = "english",
				PersianTranslation = "فارسی",
				WordTypeId = 1,
				VerbTenseId = 1,
			};

			var users =
				await unitOfWork.UserRepository.GetAllAsync();

			var user = users.FirstOrDefault();

			WordServices.HttpContextAccessor.HttpContext = new DefaultHttpContext();

			WordServices.HttpContextAccessor.HttpContext.Items["User"] = user;

			var result =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			AddWordRequestViewModel addWordRequestViewModel2 = new AddWordRequestViewModel()
			{
				Word = word + "b",
				Source = "test",
				EnglishTranslation = "english",
				PersianTranslation = "فارسی",
				WordTypeId = 1,
				VerbTenseId = 1,
			};

			var result2 =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel2);


			var examProcessingRequestViewModels = new List<ExamProcessingRequestViewModel>();

			examProcessingRequestViewModels.Add(new ExamProcessingRequestViewModel()
			{
				Question = addWordRequestViewModel.Word,
				Answer = addWordRequestViewModel.EnglishTranslation,
				Language = "english"
			});

			examProcessingRequestViewModels.Add(new ExamProcessingRequestViewModel()
			{
				Question = addWordRequestViewModel2.Word,
				Answer = "test",
				Language = "english"
			});

			//Act
			var result3 =
				await WordServices.ExamProcessing(examProcessingRequestViewModels);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.ExamProcessingSuccessful);

			//Assert
			Assert.Equal(expected: successMessage, actual: result3.Successes[0]);
			Assert.NotNull(result3.Value);
			Assert.NotNull(result3.Value.PrimitiveResults);
			Assert.NotNull(result3.Value.CompleteResult);

			Assert.Equal(expected: addWordRequestViewModel.Word, actual: result3.Value.PrimitiveResults[0].Question);
			Assert.Equal(expected: addWordRequestViewModel.EnglishTranslation, actual: result3.Value.PrimitiveResults[0].YourAnswer);
			Assert.Equal(expected: addWordRequestViewModel.EnglishTranslation, actual: result3.Value.PrimitiveResults[0].CorrectAnswer);
			Assert.True(result3.Value.PrimitiveResults[0].IsCorrect);

			Assert.Equal(expected: addWordRequestViewModel2.Word, actual: result3.Value.PrimitiveResults[1].Question);
			Assert.Equal(expected: "test", actual: result3.Value.PrimitiveResults[1].YourAnswer);
			Assert.Equal(expected: addWordRequestViewModel2.EnglishTranslation, actual: result3.Value.PrimitiveResults[1].CorrectAnswer);
			Assert.False(result3.Value.PrimitiveResults[1].IsCorrect);
		}
		#endregion /ExamProcessing
	}
}
