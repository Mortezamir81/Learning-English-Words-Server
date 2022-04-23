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
	internal class PrimitiveResultConfiguration : IEntityTypeConfiguration<PrimitiveResult>
	{
		public PrimitiveResultConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<PrimitiveResult> builder)
		{
			builder.ToTable
				("PrimitiveResults", "LE")
					.HasKey(current => current.Id);
		}
	}
}
