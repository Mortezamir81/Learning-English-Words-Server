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
	internal class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
	{
		public UserLoginConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<UserLogin> builder)
		{
			builder.ToTable
				("UserLogins", "LE")
					.HasKey(current => current.Id);


		}
	}
}
