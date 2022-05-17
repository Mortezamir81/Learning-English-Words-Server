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
			var result =
				await WordServices.GetWord(word: word);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}


		[HttpGet("recent")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> GetRecentLearned(string word)
		{
			var result =
				await WordServices.GetRecentLearningWords();

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}


		[HttpGet("exam")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> GetExam([FromQuery] GetExamRequestViewModel getExamRequestViewModel)
		{
			var result =
				await WordServices.GetExam(getExamRequestViewModel);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}


		[HttpGet]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> GetAllWords([FromQuery] GetAllWordsRequestViewModel getAllWordsRequestViewModel)
		{
			var result =
				await WordServices.GetAllWords(getAllWordsRequestViewModel);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		#endregion /HttpGet

		#region HttpPost
		[HttpPost]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> AddWord(AddWordRequestViewModel addWordRequestViewModel)
		{
			var result = 
				await WordServices.AddNewWord(addWordRequestViewModel: addWordRequestViewModel);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}


		[HttpPost("exam")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> ExamProcessing(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels)
		{
			var result =
				await WordServices.ExamProcessing(examProcessingRequestViewModels);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		#endregion /HttpPost

		#region HttpPut
		[HttpPut]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> UpdateWord(AddWordRequestViewModel addWordRequestViewModel)
		{
			var result =
				await WordServices.UpdateWord(word: addWordRequestViewModel);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		#endregion /HttpPut

		#region HttpDelete
		[HttpDelete("{word}")]
		[Authorize(UserRoles.All)]
		public async Task<IActionResult> RemoveWord(string word)
		{
			var result =
				await WordServices.RemoveWord(word: word);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		#endregion /HttpDelete
	}
}
