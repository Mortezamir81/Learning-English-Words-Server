using Microsoft.AspNetCore.Http;
using Softmax.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Softmax.Utilities
{
	public class LogUtilities
	{
		public static Hashtable GetProperties(object instance)
		{
			try
			{
				if (instance == null)
					return null;

				var result = new Hashtable();

				var type = instance.GetType();
				var properties = type.GetProperties();

				foreach (var property in properties)
				{
					if (!property.CanRead)
						continue;

					if (property.Name == "Password")
						continue;

					object value = property.GetValue(instance);

					result.Add(property.Name, value);
				}

				return result;
			}
			catch 
			{
				return null;
			}
		}
	}
}
