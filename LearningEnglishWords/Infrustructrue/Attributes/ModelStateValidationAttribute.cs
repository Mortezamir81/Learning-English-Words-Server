namespace Infrustructrue.Attributes
{
	public class ModelStateValidationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.ModelState.IsValid == false)
			{
				var result = new Result();

				foreach (var modelState in context.ModelState)
				{		
					foreach (var error in modelState.Value.Errors)
					{
						result.AddErrorMessage(error.ErrorMessage);
					}
				}

				context.Result = 
					new BadRequestObjectResult(result);
			}

			base.OnActionExecuting(context);
		}
	}
}
