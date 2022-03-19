using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base
{
	public abstract class Entity : object
	{
		public Entity() : base()
		{
			Id = System.Guid.NewGuid();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		private System.Guid? _id;

		public System.Guid? Id
		{
			get
			{
				return _id;
			}
			set
			{
				if (value == null)
				{
					_id = System.Guid.NewGuid();
				}
				else
				{
					_id = value;
				}
			}
		}
	}
}
