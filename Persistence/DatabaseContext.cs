using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{

		}

		public DbSet<Roles> Roles { get; set; }
		public DbSet<VerbTenses> VerbTenses { get; set; }
		public DbSet<WordTypes> WordTypes { get; set; }
		public DbSet<Exams> Exams { get; set; }
		public DbSet<UserLogins> UserLogins { get; set; }
		public DbSet<Users> Users { get; set; }
		public DbSet<Notifications> Notifications { get; set; }
		public DbSet<ApplicationVersions> ApplicationVersions { get; set; }
		public DbSet<Ticket> Tickets { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<User>().ToTable("Users");
			//modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		}
	}
}
