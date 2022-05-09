using Microsoft.AspNetCore.Http;
using System.Net;
using ViewModels.General;

namespace Infrastructure.Middlewares
{
	public class CustomStaticFilesMiddleware : object
	{
		public CustomStaticFilesMiddleware
			(RequestDelegate next, Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment) : base()
		{
			Next = next;
			HostEnvironment = hostEnvironment;
		}

		private RequestDelegate Next { get; }

		private Microsoft.Extensions.Hosting.IHostEnvironment HostEnvironment { get; }

		public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
		{
			string requestPath =
				httpContext.Request.Path;

			if (string.IsNullOrWhiteSpace(requestPath) || requestPath == "/")
			{
				await Next(httpContext);
				return;
			}

			if (requestPath.StartsWith("/") == false)
			{
				await Next(httpContext);
				return;
			}

			requestPath =
				requestPath[1..];


			var rootPath =
				HostEnvironment.ContentRootPath;

			var physicalPathName =
				System.IO.Path.Combine
				(path1: rootPath, path2: "wwwroot", path3: requestPath);


			if (System.IO.File.Exists(physicalPathName) == false)
			{
				await Next(httpContext);
				return;
			}

			string fileExtension =
				System.IO.Path.GetExtension(physicalPathName)?.ToLower();

			switch (fileExtension)
			{		
				case ".jpg":
				case ".jpeg":
					{
						if (httpContext.Items["User"] as UserInformationInToken == null)
						{
							httpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
							await httpContext.Response.WriteAsync("Unauthorized");

							return;
						}

						httpContext.Response.StatusCode = 200;
						httpContext.Response.ContentType = "image/jpeg";

						break;
					}

				case ".png":
					{
						if (httpContext.Items["User"] as UserInformationInToken == null)
						{
							httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
							await httpContext.Response.WriteAsync("Unauthorized");

							return;
						}

						httpContext.Response.StatusCode = 200;
						httpContext.Response.ContentType = "image/png";

						break;
					}

				default:
					{
						await Next(httpContext);
						return;
					}
			}

			await httpContext.Response
				.SendFileAsync(fileName: physicalPathName);
		}
	}
}