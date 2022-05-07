using AutoMapper;
using Persistence;
using Infrustructrue.Settings;
using Infrustructrue.Utilities;
using Services;
using ViewModels.Requests;
using ViewModels.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Dtat.Logging;
using Dtat.Logging.NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace MaxLearnTest.Services
{
	public class UserServicesUnitTests
	{
		public UserServices UserServices { get; set; }
		public UserServicesUnitTests()
		{
			DbContextOptionsBuilder options = new DbContextOptionsBuilder();
			options.UseSqlServer(connectionString: "Data Source = 62.204.61.142;  Initial Catalog = mbteamir_LEW; User ID = mbteamir_LEW_Admin; Password = Mm@13811386;");

			DatabaseContext databaseContext = new DatabaseContext(options.Options);

			UnitOfWork unitOfWork = new UnitOfWork(databaseContext);

			ILogger<UserServices> loggerForUserServices = new NLogAdapter<UserServices>(null);

			var config = new MapperConfiguration(cfg => {
				cfg.AddProfile<Infrustructrue.AutoMapperProfiles.UserProfile>();
			});

			Mapper mapper = new Mapper(config);

			ILogger<TokenUtility> loggerForTokenUtility = new NLogAdapter<TokenUtility>(null);

			TokenUtility tokenUtility = new TokenUtility(logger: loggerForTokenUtility, unitOfWork: unitOfWork);

			var jwtSettings = new JwtSettings()
			{
				SecretKeyForToken = "sssssssssssssssssssssssssssssssssssssssss",
				TokenExpiresTime = 8
			};

			var settings = new ApplicationSettings()
			{
				JwtSettings = jwtSettings
			};

			IOptions<ApplicationSettings> appSettingsOptions = Options.Create(settings);

			HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            UserServices =
                new UserServices
                    (unitOfWork: unitOfWork,
                    logger: loggerForUserServices,
                    mapper: mapper, tokenUtility: tokenUtility,
                    options: appSettingsOptions,
					databaseContext: databaseContext,
					httpContextAccessor: httpContextAccessor);
        }

		#region Login
		[Fact]
		public async Task TestLoginWithNullUsername()
		{
			//Arrange
			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(loginRequestViewModel.Username), nameof(loginRequestViewModel));

			//Act
			var result =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestLoginWithNullStringUsername()
		{
			//Arrange
			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = ""
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(loginRequestViewModel.Username), nameof(loginRequestViewModel));

			//Act
			var result =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestLoginWithNullPassword()
		{
			//Arrange
			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = "Test",
				Password = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(loginRequestViewModel.Password), nameof(loginRequestViewModel));

			//Act
			var result =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestLoginWithNullStringPassword()
		{
			//Arrange
			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = "Test",
				Password = ""
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(loginRequestViewModel.Password), nameof(loginRequestViewModel));

			//Act
			var result =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestLoginWithNullValues()
		{
			//Arrange
			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = null,
				Password = null
			};

			string usernameErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(loginRequestViewModel.Username), nameof(loginRequestViewModel));


			string passwordErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(loginRequestViewModel.Password), nameof(loginRequestViewModel));

			//Act
			var result =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			var usernameErrorResult =
				result.Errors.Where(current => current == usernameErrorMessage).SingleOrDefault();

			var passwordErrorResult =
				result.Errors.Where(current => current == passwordErrorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: usernameErrorMessage, actual: usernameErrorResult);
			Assert.Equal(expected: passwordErrorResult, actual: passwordErrorResult);
		}

		[Fact]
		public async Task TestLoginWithInvalidUsername()
		{
			//Arrange
			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = Guid.NewGuid().ToString(),
				Password = "1234"
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

			//Act
			var result =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestLoginWithInvalidPassword()
		{
			//Arrange

			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = Guid.NewGuid().ToString(),
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = username,
				Password = "1234"
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

			//Act
			var result2 =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result2.Errors[0]);
		}

		[Fact]
		public async Task TestLoginWithInvalidUsernameAndPassword()
		{
			//Arrange
			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = Guid.NewGuid().ToString(),
				Password = Guid.NewGuid().ToString()
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

			//Act
			var result2 =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result2.Errors[0]);
		}

		[Fact]
		public async Task TestLoginWithCorrectValues()
		{
			//Arrange

			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
			{
				Username = username,
				Password = password
			};

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoginSuccessful);

			//Act
			var result2 =
				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

			var loginUsername =
				result2.Value.Username == registerRequestViewModel.Username;

			var loginEmail =
				result2.Value.Email == registerRequestViewModel.Email;

			var loginToken = result2.Value.Token;

			//Assert
			Assert.Equal(expected: successMessage, actual: result2.Successes[0]);
			Assert.True(loginUsername);
			Assert.True(loginEmail);
			Assert.NotNull(loginToken);
			Assert.NotEmpty(loginToken);
		}
		#endregion /Login

		#region Register
		[Fact]
		public async Task TestRegisterWithNullUsername()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = null,
				Email = "Test@gmail.com",
				Password = "Test"
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(registerRequestViewModel.Username), nameof(registerRequestViewModel));

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestRegisterWithNullStringUsername()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = "",
				Email = "Test@gmail.com",
				Password = Guid.NewGuid().ToString()
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(registerRequestViewModel.Username), nameof(registerRequestViewModel));

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestRegisterWithNullPassword()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = "Test",
				Email = "Test@gmail.com",
				Password = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(registerRequestViewModel.Password), nameof(registerRequestViewModel));

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestRegisterWithNullStringPassword()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = "Test",
				Email = "Test@gmail.com",
				Password = ""
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(registerRequestViewModel.Password), nameof(registerRequestViewModel));

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestRegisterWithNullEmail()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = "Test",
				Email = null,
				Password = Guid.NewGuid().ToString()
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(registerRequestViewModel.Email), nameof(registerRequestViewModel));

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestRegisterWithNullStringEmail()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = "Test",
				Email = "",
				Password = "Test"
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(registerRequestViewModel.Email), nameof(registerRequestViewModel));

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			//Assert
			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		}

		[Fact]
		public async Task TestRegisterWithNullValues()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = null,
				Email = null,
				Password = null
			};

			string usernameErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(registerRequestViewModel.Username), nameof(registerRequestViewModel));

			string emailErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(registerRequestViewModel.Email), nameof(registerRequestViewModel));

			string passwordErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(registerRequestViewModel.Password), nameof(registerRequestViewModel));

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var usernameErrorResult =
				result.Errors.Where(current => current == usernameErrorMessage).SingleOrDefault();

			var emailErrorResult =
				result.Errors.Where(current => current == emailErrorMessage).SingleOrDefault();

			var passwordErrorResult =
				result.Errors.Where(current => current == passwordErrorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: usernameErrorMessage, actual: usernameErrorResult);
			Assert.Equal(expected: emailErrorMessage, actual: emailErrorResult);
			Assert.Equal(expected: passwordErrorResult, actual: passwordErrorResult);
		}

		[Fact]
		public async Task TestRegisterWithInvalidEmailStructure()
		{
			//Arrange
			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = "Test",
				Email = "Test",
				Password = Guid.NewGuid().ToString()
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidEmailStructure);

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var errorResult =
				result.Errors.Where(current => current == errorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestRegisterWithDuplicateEmail()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
			{
				Username = username,
				Email = registerRequestViewModel.Email,
				Password = Guid.NewGuid().ToString()
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.UsernameExist);

			//Act
			var result2 =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

			var errorResult =
				result2.Errors.Where(current => current == errorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestRegisterWithDuplicateUsername()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
			{
				Username = Guid.NewGuid().ToString(),
				Email = registerRequestViewModel.Email,
				Password = Guid.NewGuid().ToString()
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.EmailExist);

			//Act
			var result2 =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

			var errorResult =
				result2.Errors.Where(current => current == errorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestRegisterWithCorrectValues()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.RegisterSuccessful);

			//Act
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var errorResult =
				result.Successes.Where(current => current == successMessage).SingleOrDefault();

			var createdUser =
				await UserServices.GetByUsernameAsync(username: username);

			//Assert
			Assert.Equal(expected: successMessage, actual: errorResult);
			Assert.NotNull(createdUser);
		}
		#endregion /Register

		#region GetAllUsers
		//[Fact]
		//public async Task TestGetAllUsersWhenNoUserExistInDatabase()
		//{
		//	//Arrange
		//	var users = 
		//		await UserServices.UnitOfWork.UserRepository.GetAllAsync();

		//	if (users != null)
		//	{
		//		if (users.Count() > 0)
		//		{
		//			foreach (var user in users)
		//			{
		//				await UserServices.UnitOfWork.UserRepository.RemoveByIdAsync(user.Id);
		//				await UserServices.UnitOfWork.SaveAsync();
		//			}
		//		}
		//	}

		//	string errorMessage = string.Format
		//		(Resources.Messages.ErrorMessages.UserListEmpty);

		//	//Act
		//	var result = await UserServices.GetAllUsersAsync();

		//	//Assert
		//	Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
		//}

		[Fact]
		public async Task TestGetAllUsersWhenUserExistInDatabase()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.LoadUsersSuccessfull);

			//Act
			var result2 = await UserServices.GetAllUsersAsync();

			var result3 = result2.Value;

			//Assert
			Assert.Equal(expected: successMessage, actual: result2.Successes[0]);
			Assert.NotNull(result3);
		}
		#endregion /GetAllUsers

		#region UpdateUserAsync
		[Fact]
		public async Task TestUpdateUserWithNullId()
		{
			//Arrange
			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Id = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(updateUserRequestViewModel.Id), nameof(updateUserRequestViewModel));

			//Act
			var result =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var errorResult =
				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithNullUsername()
		{
			//Arrange
			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Username = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(updateUserRequestViewModel.Username), nameof(updateUserRequestViewModel));

			//Act
			var result =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var errorResult =
				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithNullEmail()
		{
			//Arrange
			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Email = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(updateUserRequestViewModel.Email), nameof(updateUserRequestViewModel));

			//Act
			var result =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var errorResult =
				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithNullValues()
		{
			//Arrange
			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Email = null,
				Username = null,
				Id = null
			};

			string emailErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.Email), nameof(updateUserRequestViewModel));

			string usernameErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.Username), nameof(updateUserRequestViewModel));

			string idErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(updateUserRequestViewModel.Id), nameof(updateUserRequestViewModel));

			//Act
			var result =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var usernameErrorResult =
				result.Errors.Where(current => current == usernameErrorMessage).SingleOrDefault();

			var emailErrorResult =
				result.Errors.Where(current => current == emailErrorMessage).SingleOrDefault();

			var idErrorResult =
				result.Errors.Where(current => current == idErrorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: usernameErrorMessage, actual: usernameErrorResult);
			Assert.Equal(expected: emailErrorMessage, actual: emailErrorResult);
			Assert.Equal(expected: idErrorMessage, actual: idErrorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithInvalidEmailStructure()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var createdUser =
				await UserServices.GetByUsernameAsync(username: username);

			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Id = createdUser.Id,
				Username = createdUser.Username,
				Email = "Test"
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidEmailStructure);

			//Act
			var result2 =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var errorResult =
				result2.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithInvalidRoleId()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var createdUser =
				await UserServices.GetByUsernameAsync(username: username);

			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Id = createdUser.Id,
				Username = createdUser.Username,
				Email = "Test",
				RoleId = 5
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.RoleNotFound);

			//Act
			var result2 =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var errorResult =
				result2.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithExistingEmail()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var createdUser1 =
				await UserServices.GetByUsernameAsync(username: username);

			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
			{
				Username = username + "Test",
				Password = password,
				Email = $"Test1{random1}@gmail.com"
			};

			var result2 =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

			var createdUser2 =
				await UserServices.GetByUsernameAsync(username: registerRequestViewModel2.Username);

			//Arrange
			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Id = createdUser1.Id,
				Username = createdUser1.Username,
				Email = createdUser2.Email
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.EmailExist);

			//Act
			var result3 =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var errorResult =
				result3.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithExistingUsername()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var createdUser1 =
				await UserServices.GetByUsernameAsync(username: username);

			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
			{
				Username = username + "Test",
				Password = password,
				Email = $"Test1{random1}@gmail.com"
			};

			var result2 =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

			var createdUser2 =
				await UserServices.GetByUsernameAsync(username: registerRequestViewModel2.Username);

			//Arrange
			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Id = createdUser1.Id,
				Username = createdUser2.Username,
				Email = createdUser1.Email
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.UsernameExist);

			//Act
			var result3 =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var errorResult =
				result3.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithInvalidValues()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			var registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var registerRequestViewModel2 = new RegisterRequestViewModel()
			{
				Username = username + "Test",
				Password = password,
				Email = $"Test1{random1}@gmail.com"
			};

			var result2 =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

			var createdUser =
				await UserServices.GetByUsernameAsync(username: username);

			var createdUser2 =
				await UserServices.GetByUsernameAsync(username: registerRequestViewModel2.Username);

			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Id = createdUser.Id,
				Email = "Test",
				RoleId = -1,
				Username = createdUser2.Username
			};

			string emailErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.InvalidEmailStructure);

			string roleErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.RoleNotFound);

			string userErrorMessage = string.Format
				(Resources.Messages.ErrorMessages.UsernameExist);

			//Act
			var result3 =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var usernameErrorResult =
				result3.Errors.Where(current => current == userErrorMessage).SingleOrDefault();

			var emailErrorResult =
				result3.Errors.Where(current => current == emailErrorMessage).SingleOrDefault();

			var roleErrorResult =
				result3.Errors.Where(current => current == roleErrorMessage).SingleOrDefault();

			//Assert
			Assert.Equal(expected: userErrorMessage, actual: usernameErrorResult);
			Assert.Equal(expected: emailErrorMessage, actual: emailErrorResult);
			Assert.Equal(expected: roleErrorMessage, actual: roleErrorResult);
		}

		[Fact]
		public async Task TestUpdateUserWithCorrectValues()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var createdUser =
				await UserServices.GetByUsernameAsync(username: username);

			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
			{
				Id = createdUser.Id,
				Username = createdUser.Username,
				Email = $"Test1{random1}@gmail.com",
				RoleId = createdUser.RoleId,
				IsDeleted = true
			};

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.UpdateSuccessful);

			//Act
			var result3 =
				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			var users =
				await UserServices.UnitOfWork.UserRepository.GetAllAsync();

			var updatedUser =
				users.Where(current => current.Id == updateUserRequestViewModel.Id &&
				current.Username == updateUserRequestViewModel.Username &&
				current.Email == updateUserRequestViewModel.Email &&
				current.IsDeleted == updateUserRequestViewModel.IsDeleted);

			var successResult =
				result3.Successes.Where(current => current == successMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: successMessage, actual: successResult);
			Assert.NotNull(updatedUser);
		}
		#endregion /UpdateUserAsync

		#region DeleteUserAsync
		[Fact]
		public async Task TestDeleteUserWithNullId()
		{
			//Arrange
			var deleteUserRequestViewModel = new DeleteUserRequestViewModel()
			{
				Id = null
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
				nameof(deleteUserRequestViewModel.Id), nameof(deleteUserRequestViewModel));

			//Act
			var result =
				await UserServices.DeleteUsersAsync(deleteUserRequestViewModel: deleteUserRequestViewModel);

			var errorResult =
				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestDeleteUserWithIncorrectId()
		{
			//Arrange
			var deleteUserRequestViewModel = new DeleteUserRequestViewModel()
			{
				Id = Guid.NewGuid()
			};

			string errorMessage = string.Format
				(Resources.Messages.ErrorMessages.UserNotFound);

			//Act
			var result =
				await UserServices.DeleteUsersAsync(deleteUserRequestViewModel: deleteUserRequestViewModel);

			var errorResult =
				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

			//Assert
			Assert.Equal(expected: errorMessage, actual: errorResult);
		}

		[Fact]
		public async Task TestDeleteUserWithCorrectId()
		{
			//Arrange
			string username = Guid.NewGuid().ToString();

			string random = username.Replace("-", "");

			string random1 = random.Remove(0, 15);

			string password = Guid.NewGuid().ToString();

			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
			{
				Username = username,
				Password = password,
				Email = $"Test{random1}@gmail.com"
			};

			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			var createdUser =
				await UserServices.GetByUsernameAsync(username: username);

			var deleteUserRequestViewModel = new DeleteUserRequestViewModel()
			{
				Id = createdUser.Id
			};

			string successMessage = string.Format
				(Resources.Messages.SuccessMessages.DeleteUserSuccessful);

			//Act
			var result2 =
				await UserServices.DeleteUsersAsync(deleteUserRequestViewModel: deleteUserRequestViewModel);

			var errorResult =
				result2.Successes.Where(current => current == successMessage).FirstOrDefault();

			var deletedUser =
				await UserServices.GetByUsernameAsync(username: username);

			var isUserDeleted = deletedUser.IsDeleted == true;

			//Assert
			Assert.Equal(expected: successMessage, actual: errorResult);
			Assert.True(isUserDeleted);
		}
		#endregion /DeleteUserAsync
	}
}
