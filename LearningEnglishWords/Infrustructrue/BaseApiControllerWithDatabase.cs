namespace Infrustructrue
{
	[ApiController]
	[Route("api/[controller]")]
	public class BaseApiControllerWithDatabase : ControllerBase
	{
		public BaseApiControllerWithDatabase(IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}


		public IUnitOfWork UnitOfWork { get; set; }
	}
}
