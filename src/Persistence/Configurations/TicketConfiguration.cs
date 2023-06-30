using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
	internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
	{
		public TicketConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<Ticket> builder)
		{
			builder.Property
				(current => current.Message)
					.IsRequired();

			builder.Property
				(current => current.Method)
					.IsRequired();

			builder.Property
				(current => current.SentDate)
					.HasDefaultValueSql("getutcdate()")
					.HasColumnType("datetime")
					.IsRequired();
		}
	}
}
