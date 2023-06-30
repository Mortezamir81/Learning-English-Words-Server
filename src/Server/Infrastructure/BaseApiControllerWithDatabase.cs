namespace Infrastructure
{
	[ApiController]
	[ApiVersion("1")]
	[Produces("application/json")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class BaseApiControllerWithDatabase : ControllerBase
	{
		public BaseApiControllerWithDatabase(IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}


		public IUnitOfWork UnitOfWork { get; set; }


		[NonAction]
		protected Guid? GetUserId()
		{
			if (User?.Identity?.IsAuthenticated == false)
				return default;

			var stringUserId =
				User?.Claims.FirstOrDefault(current => current.Type == ClaimTypes.NameIdentifier)?.Value;

			if (stringUserId == null)
				return default;

			var userId = Guid.Parse(stringUserId);

			return userId;
		}


		[NonAction]
		protected Guid GetRequierdUserId()
		{
			if (User?.Identity?.IsAuthenticated == false)
				throw new Exception("The user is not authenticated!");

			var stringUserId =
				User?.Claims.FirstOrDefault(current => current.Type == ClaimTypes.NameIdentifier)?.Value;

			if (stringUserId == null)
				throw new Exception("The user is not authenticated!");

			var userId = Guid.Parse(stringUserId);

			return userId;
		}
	}
}
