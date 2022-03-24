using Dtat.Logging;
using Dtat.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Server.Infrustructrue.Middlewares
{
    public class GlobalExceptionMiddleware
    {
		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger) : base()
		{
			Next = next;
            Logger = logger;
        }


		protected RequestDelegate Next { get; }
        public ILogger<GlobalExceptionMiddleware> Logger { get; }


        public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await Next(httpContext);
			}
			catch (Exception ex)
			{
				//******************************
				await Logger.LogCritical(ex, ex.Message);
				//******************************

				//******************************
				httpContext.Response.StatusCode = 500;

				httpContext.Response.ContentType = "application/json";

				var result = new Result();

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.UnkonwnError);

				result.AddErrorMessage(errorMessage);

				await httpContext.Response.WriteAsJsonAsync(result);
				//******************************
			}
		}
	}
}
