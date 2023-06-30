namespace Server.Controllers.V1
{
	public class WordsController : BaseApiControllerWithDatabase
	{
		#region Constractor
		public WordsController
			(IUnitOfWork unitOfWork,
			IWordServices wordServices,
			ILogger<WordsController> logger) : base(unitOfWork)
		{
			Logger = logger;
			WordServices = wordServices;
		}
		#endregion /Constractor

		#region Properties
		public IWordServices WordServices { get; }
		public ILogger<WordsController> Logger { get; }
		public IHttpContextAccessor HttpContextAccessor { get; }
		#endregion /Properties

		#region HttpGet
		[HttpGet("{word}")]
		[Authorize]
		public async Task<IActionResult> GetWord(string word)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.GetWord(word: word, userId: userId);

			return serviceResult.ApiResult();
		}


		[HttpGet("recent")]
		[Authorize]
		public async Task<IActionResult> GetRecentLearned()
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.GetRecentLearningWords(userId: userId);

			return serviceResult.ApiResult();
		}


		[HttpGet("exam")]
		[Authorize]
		public async Task<IActionResult> GetExam([FromQuery] GetExamRequestViewModel getExamRequestViewModel)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.GetExam(getExamRequestViewModel, userId: userId);

			return serviceResult.ApiResult();
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllWords([FromQuery] GetAllWordsRequestViewModel getAllWordsRequestViewModel)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.GetAllWords(getAllWordsRequestViewModel, userId: userId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpGet

		#region HttpPost
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddWord(AddWordRequestViewModel addWordRequestViewModel)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel, userId: userId);

			return serviceResult.ApiResult();
		}


		[HttpPost("exam")]
		[Authorize]
		public async Task<IActionResult> ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.ExamProcessing(examProcessingRequestViewModels, userId: userId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpPost

		#region HttpPut
		[HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateWord(AddWordRequestViewModel addWordRequestViewModel)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.UpdateWord(word: addWordRequestViewModel, userId: userId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpPut

		#region HttpDelete
		[HttpDelete("{word}")]
		[Authorize]
		public async Task<IActionResult> RemoveWord(string word)
		{
			var userId = GetRequierdUserId();

			var serviceResult =
				await WordServices.RemoveWord(word: word, userId: userId);

			return serviceResult.ApiResult();
		}
		#endregion /HttpDelete
	}
}
