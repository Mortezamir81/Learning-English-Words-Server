using Dtat.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtat.Results
{
	public class ResponseResult<T> : ResponseResult
	{
		public ResponseResult() : base()
		{
		}

		public T Value { get; set; }
	}
}
