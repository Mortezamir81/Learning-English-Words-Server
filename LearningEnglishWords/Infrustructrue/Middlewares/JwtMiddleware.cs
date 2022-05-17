namespace Infrustructrue.Middlewares
{
	public class JwtMiddleware
	{
		public JwtMiddleware(RequestDelegate next, IOptions<ApplicationSettings> options) : base()
		{
			Next = next;
			ApplicationSettings = options.Value;
		}

		protected RequestDelegate Next { get; }
		protected ApplicationSettings ApplicationSettings { get; }

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
					(context: context, token: token, secretKey: ApplicationSettings.JwtSettings.SecretKeyForToken);

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
