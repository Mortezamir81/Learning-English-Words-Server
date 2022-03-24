using System;

namespace Domain.Entities
{
	public class ApplicationVersions : Base.Entity
	{
		public ApplicationVersions() : base()
		{
		}


		public string Link { get; set; }
		public string Version { get; set; }
		public DateTime PublishDate { get; set; }
	}
}
