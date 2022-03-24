using Dtat.Logging;
using Infrustructrue.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.General;

namespace Infrustructrue.Utilities
{
	public class TokenUtility : ITokenUtility
	{
		public TokenUtility(ILogger<TokenUtility> logger, Persistence.IUnitOfWork unitOfWork)
		{
			Logger = logger;
			UnitOfWork = unitOfWork;
		}


		private ILogger<TokenUtility> Logger { get; }
		public Persistence.IUnitOfWork UnitOfWork { get; }


		public async Task AttachUserToContextByToken
			(HttpContext context, string token, string secretKey)
		{
			try
			{
				var key =
					Encoding.ASCII.GetBytes(secretKey);

				var tokenHandler =
					new JwtSecurityTokenHandler();

				tokenHandler.ValidateToken(token: token,
					validationParameters: new TokenValidationParameters()
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateIssuerSigningKey = true,

						IssuerSigningKey =
							new SymmetricSecurityKey(key: key),

						ClockSkew =
							TimeSpan.Zero
					}, out SecurityToken validatedToken);

				var jwtToken =
					validatedToken as JwtSecurityToken;

				if (jwtToken == null)
					return;

				Claim securityStampClaim =
					jwtToken.Claims
					.Where(current => current.Type.ToLower() == nameof(Users.SecurityStamp).ToLower())
					.FirstOrDefault();

				Claim usernameClaim =
					jwtToken.Claims
					.Where(current => current.Type.ToLower() == nameof(Users.Username).ToLower())
					.FirstOrDefault();

				Claim roleIdClaim =
					jwtToken.Claims
					.Where(current => current.Type.ToLower() == nameof(Users.RoleId).ToLower())
					.FirstOrDefault();

				Claim userIdClaim =
					jwtToken.Claims
					.Where(current => current.Type.ToLower() == nameof(Users.Id).ToLower())
					.FirstOrDefault();

				if (securityStampClaim == null)
					return;

				if (!Guid.TryParse(securityStampClaim.Value, out var securityStamp))
					return;

				bool isExistSecurityStamp =
					await UnitOfWork.UserRepository.CheckUserSecurityStampAsync(securityStamp);

				if (isExistSecurityStamp == false)
					return;

				if (!Guid.TryParse(userIdClaim.Value, out var userId))
					return;

				var userInformationInToken = new UserInformationInToken()
				{
					Id = userId,
					Username = usernameClaim.Value,
					RoleId = Convert.ToInt32(roleIdClaim.Value),
				};

				context.Items["User"] = userInformationInToken;
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("Lifetime"))
				{
					return;
				}

				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidJwtToken);

				await Logger.LogError(ex, errorMessage);
			}
		}


		private SigningCredentials GetSigningCredentional(string securityKey)
		{
			byte[] key =
				Encoding.ASCII.GetBytes(securityKey);

			var symmetricSecurityKey =
				new SymmetricSecurityKey(key: key);

			var securityAlgorithm =
				SecurityAlgorithms.HmacSha256Signature;

			var signingCredentional =
				new SigningCredentials(key: symmetricSecurityKey, algorithm: securityAlgorithm);

			return signingCredentional;
		}


		public string GenerateJwtToken
			(ApplicationSettings applicationSettings, string securityKey, ClaimsIdentity claimsIdentity, DateTime dateTime)
		{
			var signingCredentional =
				GetSigningCredentional(securityKey);

			var tokenDescriptor =
				GetTokenDescriptor(claimsIdentity, dateTime, signingCredentional);

			var tokenHandler = new JwtSecurityTokenHandler();

			SecurityToken securityToken =
				tokenHandler.CreateToken(tokenDescriptor);

			string token =
				tokenHandler.WriteToken(securityToken);

			return token;
		}


		private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claimsIdentity, DateTime dateTime, SigningCredentials signingCredentional)
		{
			var tokenDescriptor =
				new SecurityTokenDescriptor()
				{
					Subject = claimsIdentity,

					//Issuer = "",
					//Audience = "",

					Expires = dateTime,
					SigningCredentials = signingCredentional
				};

			return tokenDescriptor;
		}
	}
}
