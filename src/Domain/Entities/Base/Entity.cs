using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base
{
	public abstract class Entity : object
	{
		public Entity()
		{
			Id = Guid.NewGuid();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.Guid Id { get; set; }
	}
}
