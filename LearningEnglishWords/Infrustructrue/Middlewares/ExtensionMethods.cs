using Infrustructrue.Middlewares;
using Microsoft.AspNetCore.Builder;

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
	}
}
