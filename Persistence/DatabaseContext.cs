using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{
		}


		public DbSet<Role> Roles { get; set; }
		public DbSet<Exam> Exams { get; set; }
		public DbSet<User> Users { get; set; }
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
			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		}
	}
}
