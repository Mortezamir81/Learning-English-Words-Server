using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence.Configuration
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public UserConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable
				("Users", "LE")
				.HasKey(current => current.Id);

			builder.HasIndex
				(current => current.Email)
				.IsUnique();

			builder.Property
				(current => current.Email)
				 .IsRequired();

			builder.HasOne(e => e.Role)
				.WithMany(c => c.Users)
				.IsRequired();

			builder.Property
				(current => current.RoleId)
				.HasDefaultValue(3)
				.IsRequired();

			builder.Property
				(current => current.IsActive)
				.HasDefaultValue(false)
				.IsRequired();

			builder.Property
				(current => current.IsDeleted)
				.HasDefaultValue(false)
				.IsRequired();

			builder.Property
				(current => current.SecurityStamp)
				.IsRequired(false);

			builder.Property
				(current => current.PhoneNumber)
				.HasColumnName("UserPhoneNumber")
				.HasColumnType("int")
				.HasDefaultValue(0);

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
