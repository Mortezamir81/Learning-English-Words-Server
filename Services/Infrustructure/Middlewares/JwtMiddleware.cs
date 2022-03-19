using Infrustructrue.ApplicationSettings;
using Infrustructrue.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.General;


namespace Infrustructrue.Middlewares
{
	public class JwtMiddleware
	{
		public JwtMiddleware(RequestDelegate next, IOptions<MainSettings> options) : base()
		{
			Next = next;
			MainSettings = options.Value;
		}

		protected RequestDelegate Next { get; }
		protected MainSettings MainSettings { get; }

		public async Task InvokeAsync(HttpContext context, ITokenUtility tokenUtility)
		{
			var requestHeaders =
				context.Request.Headers["Authorization"];

			string token =
				requestHeaders
				.FirstOrDefault()?
				.Split(" ")
				.Last();

			if (string.IsNullOrEmpty(token) == false)
				await tokenUtility.AttachUserToContextByToken
					(context: context, token: token, secretKey: MainSettings.SecretKeyForToken);

			if (context.Request.Path.Value.ToLower().Contains("signal"))
			{
				if ((context.Items["User"] as UserInformationInToken) == null)
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;

					return;
				}
			}

			await Next(context);
		}
	}
}
