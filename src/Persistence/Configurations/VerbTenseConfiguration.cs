using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Persistence.Configurations
{
	internal class VerbTenseConfiguration : IEntityTypeConfiguration<VerbTense>
	{
		public VerbTenseConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<VerbTense> builder)
		{
			builder.Property
				(current => current.Tense)
				.IsRequired();

			builder.HasMany(c => c.Words)
				.WithOne(c => c.VerbTense)
				.HasForeignKey(c => c.VerbTenseId)
				.IsRequired(false);

			builder.HasData(new List<VerbTense>()
			{
				new VerbTense()
				{
					Id = 1,
					Tense = "None"
				},
				new VerbTense()
				{
					Id = 2,
					Tense = "Present Simple"
				},
				new VerbTense()
				{
					Id = 3,
					Tense = "Present Continuous"
				},
				new VerbTense()
				{
					Id = 4,
					Tense = "Present Perfect"
				},
				new VerbTense()
				{
					Id = 5,
					Tense = "Present Perfect Continuous"
				},
				new VerbTense()
				{
					Id = 6,
					Tense = "Past Simple"
				},
				new VerbTense()
				{
					Id = 7,
					Tense = "Past Continuous"
				},
				new VerbTense()
				{
					Id = 8,
					Tense = "Past Perfect"
				},
				new VerbTense()
				{
					Id = 9,
					Tense = "Past Perfect Continuous"
				},
				new VerbTense()
				{
					Id = 10,
					Tense = "Future Simple"
				},
				new VerbTense()
				{
					Id = 11,
					Tense = "Future Continuous"
				},
				new VerbTense()
				{
					Id = 12,
					Tense = "Future Perfect"
				},
				new VerbTense()
				{
					Id = 13,
					Tense = "Future Perfect Continuous"
				},
				new VerbTense()
				{
					Id = 14,
					Tense = "Future Simple in the Past"
				},
				new VerbTense()
				{
					Id = 15,
					Tense = "Future Continuous in the Past"
				},
				new VerbTense()
				{
					Id = 16,
					Tense = "Future Perfect in the Past"
				},
				new VerbTense()
				{
					Id = 17,
					Tense = "Future Perfect Continuous in the Past"
				},
			});
		}
	}
}
