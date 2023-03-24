using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public UserConfiguration() : base()
		{
		}

		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property
				(current => current.IsActive)
				.HasDefaultValue(false)
				.IsRequired();

			builder.Property
				(current => current.IsDeleted)
				.HasDefaultValue(false)
				.IsRequired();

			builder.Property
				(current => current.PhoneNumber)
				.HasColumnName("UserPhoneNumber");

			builder.Property
				(current => current.TimeRegistered)
				.HasDefaultValueSql("getutcdate()")
				.IsRequired();

			builder.Property
				(current => current.TimeUpdated)
				.IsRequired(false);
		}
	}
}
