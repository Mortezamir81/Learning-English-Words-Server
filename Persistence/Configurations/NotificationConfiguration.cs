using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
	internal class NotificationConfiguration : IEntityTypeConfiguration<Notifications>
	{
		public NotificationConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<Notifications> builder)
		{
			builder.Property
				(current => current.Message)
					.IsRequired();

			builder.Property
				(current => current.Title)
					.IsRequired();

			builder.Property
				(current => current.From)
					.IsRequired();

			builder.Property
				(current => current.Direction)
					.HasDefaultValue("ltr")
					.IsRequired();

			builder.Property
				(current => current.IsDeleted)
					.HasDefaultValue(false)
					.IsRequired();

			builder.Property
				(current => current.IsRead)
					.HasDefaultValue(false)
					.IsRequired();

			builder.Property
				(current => current.SentDate)
					.HasDefaultValueSql("getutcdate()")
					.HasColumnType("datetime")
					.IsRequired();
		}
	}
}
