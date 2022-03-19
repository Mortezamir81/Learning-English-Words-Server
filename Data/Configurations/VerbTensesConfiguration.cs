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
	internal class VerbTensesConfiguration : IEntityTypeConfiguration<VerbTenses>
	{
		public void Configure(EntityTypeBuilder<VerbTenses> builder)
		{
			builder.ToTable
				("VerbTenses", "LE")
					.HasKey(current => current.Id);

			builder.Property
				(current => current.Tense)
				.IsRequired();

			builder.HasMany(c => c.Words)
				.WithOne(c => c.VerbTense)
				.HasForeignKey(c => c.VerbTenseId)
				.IsRequired(false);

			builder.HasData(new List<VerbTenses>()
			{
				new VerbTenses()
				{
					Id = 1,
					Tense = "None"
				},
				new VerbTenses()
				{
					Id = 2,
					Tense = "Present Simple"
				},
				new VerbTenses()
				{
					Id = 3,
					Tense = "Present Continuous"
				},
				new VerbTenses()
				{
					Id = 4,
					Tense = "Present Perfect"
				},
				new VerbTenses()
				{
					Id = 5,
					Tense = "Present Perfect Continuous"
				},
				new VerbTenses()
				{
					Id = 6,
					Tense = "Past Simple"
				},
				new VerbTenses()
				{
					Id = 7,
					Tense = "Past Continuous"
				},
				new VerbTenses()
				{
					Id = 8,
					Tense = "Past Perfect"
				},
				new VerbTenses()
				{
					Id = 9,
					Tense = "Past Perfect Continuous"
				},
				new VerbTenses()
				{
					Id = 10,
					Tense = "Future Simple"
				},
				new VerbTenses()
				{
					Id = 11,
					Tense = "Future Continuous"
				},
				new VerbTenses()
				{
					Id = 12,
					Tense = "Future Perfect"
				},
				new VerbTenses()
				{
					Id = 13,
					Tense = "Future Perfect Continuous"
				},
				new VerbTenses()
				{
					Id = 14,
					Tense = "Future Simple in the Past"
				},
				new VerbTenses()
				{
					Id = 15,
					Tense = "Future Continuous in the Past"
				},
				new VerbTenses()
				{
					Id = 16,
					Tense = "Future Perfect in the Past"
				},
				new VerbTenses()
				{
					Id = 17,
					Tense = "Future Perfect Continuous in the Past"
				},
			});
		}
	}
}
