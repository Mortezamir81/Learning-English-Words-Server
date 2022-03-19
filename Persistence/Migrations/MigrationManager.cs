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
					try
					{
						if (databaseContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
						{
							databaseContext.Database.Migrate();
						}
					}
					catch (Exception ex)
					{
						//Log errors or do anything you think it's needed
						throw;
					}
				}
			}
			return host;
		}
	}
}