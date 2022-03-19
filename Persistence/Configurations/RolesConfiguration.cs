using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using System.Collections.Generic;

namespace Persistence.Configuration
{
	internal class RolesConfiguration : IEntityTypeConfiguration<Roles>
	{
		public void Configure(EntityTypeBuilder<Roles> builder)
		{
			builder.ToTable
				(name: "Roles", schema: "LE")
					.HasKey(current => current.Id);

			builder.Property
				(current => current.RoleName)
					.IsRequired();

			builder.HasData(new List<Roles>()
			{
				new Roles()
				{
					Id = 1,
					RoleName = "Admin"
				},
				new Roles()
				{
					Id = 2,
					RoleName = "Vip"
				},
				new Roles()
				{
					Id = 3,
					RoleName = "User"
				}			
			});
		}
	}
}
