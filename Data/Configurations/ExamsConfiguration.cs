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
	internal class ExamsConfiguration : IEntityTypeConfiguration<Exams>
	{
		public void Configure(EntityTypeBuilder<Exams> builder)
		{
			builder.Property
				(current => current.PocessingExamDate)
					.HasDefaultValueSql("FORMAT (getutcdate(), 'yyyy-MM-dd')")
					.HasColumnType("datetime")
					.IsRequired();
		}
	}
}
