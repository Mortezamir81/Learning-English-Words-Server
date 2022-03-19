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
	internal class WordsConfiguration : IEntityTypeConfiguration<Words>
	{
		public void Configure(EntityTypeBuilder<Words> builder)
		{
			builder.ToTable
				("Words", "LE")
					.HasKey(current => current.Id);

			builder.Property
				(current => current.Word)
					.IsRequired();

			builder.Property
				(current => current.LearningDate)
					.IsRequired();

			builder.Property
				(current => current.IsVerb)
					.IsRequired();

			builder.Property
				(current => current.Source)
					.IsRequired();

			builder.Property
				(current => current.PersianTranslation)
					.IsRequired();

			builder.Property
				(current => current.EnglishTranslation)
					.IsRequired();

			builder.Property
				(current => current.EditDate)
					.IsRequired(false);

			builder.Property
				(current => current.LearningDate)
					.HasDefaultValueSql("getutcdate()")
					.HasColumnType("datetime")
					.IsRequired();
		}
	}
}
