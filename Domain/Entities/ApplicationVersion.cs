using System;

namespace Domain.Entities
{
	public class ApplicationVersion : Base.Entity
	{
		public ApplicationVersion() : base()
		{
		}


		public string Link { get; set; }
		public string Version { get; set; }
		public DateTime PublishDate { get; set; }
	}
}
