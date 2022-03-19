using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
	internal class ApplicationVersionsConfiguration : IEntityTypeConfiguration<ApplicationVersions>
	{
		public void Configure(EntityTypeBuilder<ApplicationVersions> builder)
		{
			builder.ToTable
				("ApplicationVersions", "LE")
					.HasKey(current => current.Id);

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

			builder.HasData(new ApplicationVersions()
			{
				Version = "1.0.0.0",
				Link = "none"
			});
		}
	}
}
