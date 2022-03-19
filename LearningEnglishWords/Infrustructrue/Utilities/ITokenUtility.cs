using Infrustructrue.ApplicationSettings;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrustructrue.Utilities
{
	public interface ITokenUtility
	{
		Task AttachUserToContextByToken
			(HttpContext context, string token, string secretKey);

		string GenerateJwtToken
			(MainSettings mainSettings, string securityKey, ClaimsIdentity claimsIdentity, DateTime dateTime);
	}
}
