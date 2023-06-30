using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
	internal class ApplicationVersionConfiguration : IEntityTypeConfiguration<ApplicationVersion>
	{
		public ApplicationVersionConfiguration() : base()
		{
		}

		public void Configure(EntityTypeBuilder<ApplicationVersion> builder)
		{
			builder.Property
				(current => current.Version)
					.IsRequired();

			builder.Property
				(current => current.Link)
					.IsRequired();

			builder.Property
				(current => current.PublishDate)
					.HasDefaultValueSql("getutcdate()")
					.HasColumnType("datetime")
					.IsRequired();

			builder.HasData(new ApplicationVersion()
			{
				Version = "1.0.0.0",
				Link = "none"
			});
		}
	}
}
