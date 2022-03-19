using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dtat.Utilities
{
	public class Security
	{
		public static string HashDataBySHA1(string data)
		{
			//create new instance of md5
			SHA1 sha1 = SHA1.Create();

			//convert the input text to array of bytes
			byte[] hashData =
				sha1.ComputeHash(Encoding.Default.GetBytes(data));

			//create new instance of StringBuilder to save hashed data
			StringBuilder returnValue = new StringBuilder();

			//loop for each byte and add it to StringBuilder
			for (int i = 0; i < hashData.Length; i++)
			{
				returnValue.Append(hashData[i].ToString());
			}

			// return hexadecimal string
			return returnValue.ToString();
		}
	}
}
