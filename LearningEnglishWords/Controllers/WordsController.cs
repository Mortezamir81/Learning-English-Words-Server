namespace Server.Controllers
{
	public class WordsController : BaseApiControllerWithDatabase
	{
		#region Constractor
		public WordsController
			(IUnitOfWork unitOfWork,
			IWordServices wordServices,
			Dtat.Logging.ILogger<WordsController> logger) : base(unitOfWork)
		{
			Logger = logger;
			WordServices = wordServices;
		}
		#endregion /Constractor

		#region Properties
		public IWordServices WordServices { get; }
		public Dtat.Logging.ILogger<WordsController> Logger { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		#endregion /Properties

		#region HttpGet
		[HttpGet("{word}")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> GetWord(string word)
		{
			var serviceResult =
				await WordServices.GetWord(word: word);

			return serviceResult.ApiResult();
		}


		[HttpGet("recent")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> GetRecentLearned(string word)
		{
			var serviceResult =
				await WordServices.GetRecentLearningWords();

			return serviceResult.ApiResult();
		}


		[HttpGet("exam")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> GetExam([FromQuery] GetExamRequestViewModel getExamRequestViewModel)
		{
			var serviceResult =
				await WordServices.GetExam(getExamRequestViewModel);

			return serviceResult.ApiResult();
		}


		[HttpGet]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> GetAllWords([FromQuery] GetAllWordsRequestViewModel getAllWordsRequestViewModel)
		{
			var serviceResult =
				await WordServices.GetAllWords(getAllWordsRequestViewModel);

			return serviceResult.ApiResult();
		}
		#endregion /HttpGet

		#region HttpPost
		[HttpPost]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> AddWord(AddWordRequestViewModel addWordRequestViewModel)
		{
			var serviceResult = 
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			return serviceResult.ApiResult();
		}


		[HttpPost("exam")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels)
		{
			var serviceResult =
				await WordServices.ExamProcessing(examProcessingRequestViewModels);

			return serviceResult.ApiResult();
		}
		#endregion /HttpPost

		#region HttpPut
		[HttpPut]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> UpdateWord(AddWordRequestViewModel addWordRequestViewModel)
		{
			var serviceResult =
				await WordServices.UpdateWord(word: addWordRequestViewModel);

			return serviceResult.ApiResult();
		}
		#endregion /HttpPut

		#region HttpDelete
		[HttpDelete("{word}")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> RemoveWord(string word)
		{
			var serviceResult =
				await WordServices.RemoveWord(word: word);

			return serviceResult.ApiResult();
		}
		#endregion /HttpDelete
	}
}
