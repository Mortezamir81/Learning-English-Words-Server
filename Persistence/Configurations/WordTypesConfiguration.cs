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
	internal class WordTypesConfiguration : IEntityTypeConfiguration<WordTypes>
	{
		public WordTypesConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<WordTypes> builder)
		{
			builder.ToTable
				("WordTypes", "LE")
					.HasKey(current => current.Id);

			builder.Property
				(current => current.Type)
				.IsRequired();

			builder.HasMany(c => c.Words)
				.WithOne(c => c.WordType)
				.HasForeignKey(c => c.WordTypeId)
				.IsRequired();

			builder.HasData(new List<WordTypes>()
			{
				new WordTypes()
				{
					Id = 1,
					Type = "Noun"
				},
				new WordTypes()
				{
					Id = 2,
					Type = "Letters"
				},
				new WordTypes()
				{
					Id = 3,
					Type = "Pronoun"
				},
				new WordTypes()
				{
					Id = 4,
					Type = "Adverb"
				},
				new WordTypes()
				{
					Id = 5,
					Type = "Verb"
				},
				new WordTypes()
				{
					Id = 6,
					Type = "Adjective"
				}
			});
		}
	}
}
