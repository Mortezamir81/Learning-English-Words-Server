using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrustructrue.ApplicationSettings
{
	public class MainSettings
	{
		public string SecretKeyForToken { get; set; }

		public int TokenExpiresTime { get; set; }
	}
}
