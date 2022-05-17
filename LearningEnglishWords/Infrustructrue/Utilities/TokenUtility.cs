namespace Infrustructrue.Utilities
{
	public class TokenUtility : ITokenUtility
	{
		public TokenUtility(Dtat.Logging.ILogger<TokenUtility> logger, IUnitOfWork unitOfWork)
		{
			Logger = logger;
			UnitOfWork = unitOfWork;
		}


		public IUnitOfWork UnitOfWork { get; }
		private Dtat.Logging.ILogger<TokenUtility> Logger { get; }


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
					.Where(current => current.Type.ToLower() == nameof(User.SecurityStamp).ToLower())
					.FirstOrDefault();

				Claim roleIdClaim =
					jwtToken.Claims
					.Where(current => current.Type.ToLower() == nameof(User.RoleId).ToLower())
					.FirstOrDefault();

				Claim userIdClaim =
					jwtToken.Claims
					.Where(current => current.Type.ToLower() == nameof(User.Id).ToLower())
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
			(string securityKey, ClaimsIdentity claimsIdentity, DateTime dateTime)
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
