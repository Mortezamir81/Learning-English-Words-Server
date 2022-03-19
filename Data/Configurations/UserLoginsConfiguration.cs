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
	internal class UserLoginsConfiguration : IEntityTypeConfiguration<UserLogins>
	{
		public void Configure(EntityTypeBuilder<UserLogins> builder)
		{
			builder.ToTable
				("UserLogins", "LE")
					.HasKey(current => current.Id);
		}
	}
}
