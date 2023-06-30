using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Persistence
{
	public class DatabaseContext : IdentityDbContext<User, Role, Guid>
	{
		public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{
#if DEBUG
			Database.EnsureCreated();
#endif
		}


		public DbSet<Exam> Exams { get; set; }
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<WordType> WordTypes { get; set; }
		public DbSet<UserLogin> UserLogins { get; set; }
		public DbSet<VerbTense> VerbTenses { get; set; }
		public DbSet<Notifications> Notifications { get; set; }
		public DbSet<CompleteResult> CompleteResults { get; set; }
		public DbSet<PrimitiveResult> PrimitiveResults { get; set; }
		public DbSet<ApplicationVersion> ApplicationVersions { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var entitiesWithForeignKeys =
				modelBuilder.Model
				.GetEntityTypes()
				.SelectMany(currrent => currrent.GetForeignKeys());

			foreach (var entity in entitiesWithForeignKeys)
			{
				entity.DeleteBehavior = DeleteBehavior.Cascade;
			}

			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		}
	}
}
