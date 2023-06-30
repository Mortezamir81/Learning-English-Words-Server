using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
	internal class ExamConfiguration : IEntityTypeConfiguration<Exam>
	{
		public ExamConfiguration() : base()
		{
		}

		public void Configure(EntityTypeBuilder<Exam> builder)
		{
			builder.Property
				(current => current.PocessingExamDate)
					.HasDefaultValueSql("FORMAT (getutcdate(), 'yyyy-MM-dd')")
					.HasColumnType("datetime")
					.IsRequired();
		}
	}
}
