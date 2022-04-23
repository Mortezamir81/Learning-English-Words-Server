using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using System.Collections.Generic;

namespace Persistence.Configuration
{
	internal class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public RoleConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.ToTable
				(name: "Roles", schema: "LE")
					.HasKey(current => current.Id);

			builder.Property
				(current => current.RoleName)
					.IsRequired();

			builder.HasData(new List<Role>()
			{
				new Role()
				{
					Id = 1,
					RoleName = "Admin"
				},
				new Role()
				{
					Id = 2,
					RoleName = "Vip"
				},
				new Role()
				{
					Id = 3,
					RoleName = "User"
				}			
			});
		}
	}
}
