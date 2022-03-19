using Microsoft.AspNetCore.Mvc;

namespace Infrustructrue
{
	[ApiController]
	[Route("api/[controller]")]
	public class BaseApiControllerWithDatabase : ControllerBase
	{
		public BaseApiControllerWithDatabase(Persistence.IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}

		public Persistence.IUnitOfWork UnitOfWork { get; set; }
	}
}
