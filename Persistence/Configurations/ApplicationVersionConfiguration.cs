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
	internal class ApplicationVersionConfiguration : IEntityTypeConfiguration<ApplicationVersion>
	{
        public ApplicationVersionConfiguration():base()
        {
        }

		public void Configure(EntityTypeBuilder<ApplicationVersion> builder)
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

			builder.HasData(new ApplicationVersion()
			{
				Version = "1.0.0.0",
				Link = "none"
			});
		}
	}
}
