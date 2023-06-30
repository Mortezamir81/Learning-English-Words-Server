using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
	internal class WordConfiguration : IEntityTypeConfiguration<Word>
	{
		public WordConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<Word> builder)
		{
			builder.Property
				(current => current.Content)
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
