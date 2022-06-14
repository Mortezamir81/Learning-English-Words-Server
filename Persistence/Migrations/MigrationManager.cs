using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Migrations
{
	public static class MigrationManager
	{
		public static IHost MigrateDatabase(this IHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				using (var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
				{
					if (databaseContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
					{
						databaseContext.Database.Migrate();
					}
				}
			}

			return host;
		}
	}
}