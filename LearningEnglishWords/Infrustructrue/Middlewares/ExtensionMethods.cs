using Infrustructrue.Middlewares;
using Microsoft.AspNetCore.Builder;
using Server.Infrustructrue.Middlewares;

namespace Infrastructure.Middlewares
{
	public static class ExtensionMethods
	{
		static ExtensionMethods()
		{
		}


		public static IApplicationBuilder
			UseCustomJwtMiddleware(this IApplicationBuilder app)

		{
			return app.UseMiddleware<JwtMiddleware>();
		}

		public static IApplicationBuilder
			UseGlobalExceptionMiddleware(this IApplicationBuilder app)

		{
			return app.UseMiddleware<GlobalExceptionMiddleware>();
		}
	}
}
