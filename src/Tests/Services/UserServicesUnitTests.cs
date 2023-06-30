//using AutoMapper;
//using Persistence;
//using Infrastructure.Settings;
//using Services;
//using ViewModels.Requests;
//using ViewModels.Responses;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using Dtat.Logging;
//using Dtat.Logging.NLog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using Microsoft.AspNetCore.Http;
//using EasyCaching.Core;

//namespace MaxLearnTest.Services
//{
//	public class UserServicesUnitTests
//	{
//		public UserServices UserServices { get; set; }
//		public UserServicesUnitTests()
//		{
//			DbContextOptionsBuilder options = new DbContextOptionsBuilder();
//			options.UseSqlServer(connectionString: "Data Source = 62.204.61.142;  Initial Catalog = mbteamir_LEW; User ID = mbteamir_LEW_Admin; Password = Mm@13811386;");

//			DatabaseContext databaseContext = new DatabaseContext(options.Options);

//			UnitOfWork unitOfWork = new UnitOfWork(databaseContext);

//			ILogger<UserServices> loggerForUserServices = new NLogAdapter<UserServices>(null);

//			var config = new MapperConfiguration(cfg => {
//				cfg.AddProfile<Infrastructure.AutoMapperProfiles.UserProfile>();
//			});

//			Mapper mapper = new Mapper(config);

//			ILogger<TokenServices> loggerForTokenUtility = new NLogAdapter<TokenServices>(null);

//			TokenServices tokenUtility = new TokenServices(logger: loggerForTokenUtility, databaseContext: databaseContext);

//			var jwtSettings = new JwtSettings()
//			{
//				SecretKeyForToken = "sssssssssssssssssssssssssssssssssssssssss",
//				TokenExpiresTime = 8
//			};

//			var settings = new ApplicationSettings()
//			{
//				JwtSettings = jwtSettings
//			};

//			IOptions<ApplicationSettings> appSettingsOptions = Options.Create(settings);

//			HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

//			UserServices =
//                new UserServices
//                    (unitOfWork: unitOfWork,
//                    logger: loggerForUserServices,
//                    mapper: mapper, tokenUtility: tokenUtility,
//                    options: appSettingsOptions,
//					databaseContext: databaseContext,
//					httpContextAccessor: httpContextAccessor);
//        }

//		#region Login
//		[Fact]
//		public async Task TestLoginWithNullUserName()
//		{
//			//Arrange
//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = null
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(loginRequestViewModel.UserName), nameof(loginRequestViewModel));

//			//Act
//			var result =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestLoginWithNullStringUserName()
//		{
//			//Arrange
//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = ""
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(loginRequestViewModel.UserName), nameof(loginRequestViewModel));

//			//Act
//			var result =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestLoginWithNullPassword()
//		{
//			//Arrange
//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = "Test",
//				Password = null
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(loginRequestViewModel.Password), nameof(loginRequestViewModel));

//			//Act
//			var result =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestLoginWithNullStringPassword()
//		{
//			//Arrange
//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = "Test",
//				Password = ""
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(loginRequestViewModel.Password), nameof(loginRequestViewModel));

//			//Act
//			var result =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestLoginWithNullValues()
//		{
//			//Arrange
//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = null,
//				Password = null
//			};

//			string userNameErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(loginRequestViewModel.UserName), nameof(loginRequestViewModel));


//			string passwordErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(loginRequestViewModel.Password), nameof(loginRequestViewModel));

//			//Act
//			var result =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			var userNameErrorResult =
//				result.Errors.Where(current => current == userNameErrorMessage).SingleOrDefault();

//			var passwordErrorResult =
//				result.Errors.Where(current => current == passwordErrorMessage).SingleOrDefault();

//			//Assert
//			Assert.Equal(expected: userNameErrorMessage, actual: userNameErrorResult);
//			Assert.Equal(expected: passwordErrorResult, actual: passwordErrorResult);
//		}

//		[Fact]
//		public async Task TestLoginWithInvalidUserName()
//		{
//			//Arrange
//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = Guid.NewGuid().ToString(),
//				Password = "1234"
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

//			//Act
//			var result =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestLoginWithInvalidPassword()
//		{
//			//Arrange

//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = Guid.NewGuid().ToString(),
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = userName,
//				Password = "1234"
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

//			//Act
//			var result2 =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result2.Errors[0]);
//		}

//		[Fact]
//		public async Task TestLoginWithInvalidUserNameAndPassword()
//		{
//			//Arrange
//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = Guid.NewGuid().ToString(),
//				Password = Guid.NewGuid().ToString()
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.InvalidUserAndOrPass);

//			//Act
//			var result2 =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result2.Errors[0]);
//		}

//		[Fact]
//		public async Task TestLoginWithCorrectValues()
//		{
//			//Arrange

//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			LoginRequestViewModel loginRequestViewModel = new LoginRequestViewModel()
//			{
//				UserName = userName,
//				Password = password
//			};

//			string successMessage = string.Format
//				(Resources.Messages.SuccessMessages.LoginSuccessful);

//			//Act
//			var result2 =
//				await UserServices.LoginAsync(loginRequestViewModel: loginRequestViewModel, null);

//			var loginUserName =
//				result2.Value.UserName == registerRequestViewModel.UserName;

//			var loginEmail =
//				result2.Value.Email == registerRequestViewModel.Email;

//			var loginToken = result2.Value.Token;

//			//Assert
//			Assert.Equal(expected: successMessage, actual: result2.Successes[0]);
//			Assert.True(loginUserName);
//			Assert.True(loginEmail);
//			Assert.NotNull(loginToken);
//			Assert.NotEmpty(loginToken);
//		}
//		#endregion /Login

//		#region Register
//		[Fact]
//		public async Task TestRegisterWithNullUserName()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = null,
//				Email = "Test@gmail.com",
//				Password = "Test"
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(registerRequestViewModel.UserName), nameof(registerRequestViewModel));

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestRegisterWithNullStringUserName()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = "",
//				Email = "Test@gmail.com",
//				Password = Guid.NewGuid().ToString()
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(registerRequestViewModel.UserName), nameof(registerRequestViewModel));

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestRegisterWithNullPassword()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = "Test",
//				Email = "Test@gmail.com",
//				Password = null
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(registerRequestViewModel.Password), nameof(registerRequestViewModel));

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestRegisterWithNullStringPassword()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = "Test",
//				Email = "Test@gmail.com",
//				Password = ""
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(registerRequestViewModel.Password), nameof(registerRequestViewModel));

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestRegisterWithNullEmail()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = "Test",
//				Email = null,
//				Password = Guid.NewGuid().ToString()
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(registerRequestViewModel.Email), nameof(registerRequestViewModel));

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestRegisterWithNullStringEmail()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = "Test",
//				Email = "",
//				Password = "Test"
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(registerRequestViewModel.Email), nameof(registerRequestViewModel));

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		}

//		[Fact]
//		public async Task TestRegisterWithNullValues()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = null,
//				Email = null,
//				Password = null
//			};

//			string userNameErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//					nameof(registerRequestViewModel.UserName), nameof(registerRequestViewModel));

//			string emailErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//					nameof(registerRequestViewModel.Email), nameof(registerRequestViewModel));

//			string passwordErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//					nameof(registerRequestViewModel.Password), nameof(registerRequestViewModel));

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var userNameErrorResult =
//				result.Errors.Where(current => current == userNameErrorMessage).SingleOrDefault();

//			var emailErrorResult =
//				result.Errors.Where(current => current == emailErrorMessage).SingleOrDefault();

//			var passwordErrorResult =
//				result.Errors.Where(current => current == passwordErrorMessage).SingleOrDefault();

//			//Assert
//			Assert.Equal(expected: userNameErrorMessage, actual: userNameErrorResult);
//			Assert.Equal(expected: emailErrorMessage, actual: emailErrorResult);
//			Assert.Equal(expected: passwordErrorResult, actual: passwordErrorResult);
//		}

//		[Fact]
//		public async Task TestRegisterWithInvalidEmailStructure()
//		{
//			//Arrange
//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = "Test",
//				Email = "Test",
//				Password = Guid.NewGuid().ToString()
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.InvalidEmailStructure);

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var errorResult =
//				result.Errors.Where(current => current == errorMessage).SingleOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestRegisterWithDuplicateEmail()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Email = registerRequestViewModel.Email,
//				Password = Guid.NewGuid().ToString()
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.UserNameExist);

//			//Act
//			var result2 =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

//			var errorResult =
//				result2.Errors.Where(current => current == errorMessage).SingleOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestRegisterWithDuplicateUserName()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
//			{
//				UserName = Guid.NewGuid().ToString(),
//				Email = registerRequestViewModel.Email,
//				Password = Guid.NewGuid().ToString()
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.EmailExist);

//			//Act
//			var result2 =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

//			var errorResult =
//				result2.Errors.Where(current => current == errorMessage).SingleOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestRegisterWithCorrectValues()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			string successMessage = string.Format
//				(Resources.Messages.SuccessMessages.RegisterSuccessful);

//			//Act
//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var errorResult =
//				result.Successes.Where(current => current == successMessage).SingleOrDefault();

//			var createdUser =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			//Assert
//			Assert.Equal(expected: successMessage, actual: errorResult);
//			Assert.NotNull(createdUser);
//		}
//		#endregion /Register

//		#region GetAllUsers
//		//[Fact]
//		//public async Task TestGetAllUsersWhenNoUserExistInDatabase()
//		//{
//		//	//Arrange
//		//	var users = 
//		//		await UserServices.UnitOfWork.UserRepository.GetAllAsync();

//		//	if (users != null)
//		//	{
//		//		if (users.Count() > 0)
//		//		{
//		//			foreach (var user in users)
//		//			{
//		//				await UserServices.UnitOfWork.UserRepository.RemoveByIdAsync(user.Id);
//		//				await UserServices.UnitOfWork.SaveAsync();
//		//			}
//		//		}
//		//	}

//		//	string errorMessage = string.Format
//		//		(Resources.Messages.ErrorMessages.UserListEmpty);

//		//	//Act
//		//	var result = await UserServices.GetAllUsersAsync();

//		//	//Assert
//		//	Assert.Equal(expected: errorMessage, actual: result.Errors[0]);
//		//}

//		[Fact]
//		public async Task TestGetAllUsersWhenUserExistInDatabase()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			string successMessage = string.Format
//				(Resources.Messages.SuccessMessages.LoadUsersSuccessfull);

//			//Act
//			var result2 = await UserServices.GetAllUsersAsync();

//			var result3 = result2.Value;

//			//Assert
//			Assert.Equal(expected: successMessage, actual: result2.Successes[0]);
//			Assert.NotNull(result3);
//		}
//		#endregion /GetAllUsers

//		#region UpdateUserAsync
//		[Fact]
//		public async Task TestUpdateUserWithNullId()
//		{
//			//Arrange
//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Id = null
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(updateUserRequestViewModel.Id), nameof(updateUserRequestViewModel));

//			//Act
//			var result =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var errorResult =
//				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithNullUserName()
//		{
//			//Arrange
//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				UserName = null
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(updateUserRequestViewModel.UserName), nameof(updateUserRequestViewModel));

//			//Act
//			var result =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var errorResult =
//				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithNullEmail()
//		{
//			//Arrange
//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Email = null
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(updateUserRequestViewModel.Email), nameof(updateUserRequestViewModel));

//			//Act
//			var result =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var errorResult =
//				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithNullValues()
//		{
//			//Arrange
//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Email = null,
//				UserName = null,
//				Id = null
//			};

//			string emailErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//					nameof(updateUserRequestViewModel.Email), nameof(updateUserRequestViewModel));

//			string userNameErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//					nameof(updateUserRequestViewModel.UserName), nameof(updateUserRequestViewModel));

//			string idErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//					nameof(updateUserRequestViewModel.Id), nameof(updateUserRequestViewModel));

//			//Act
//			var result =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var userNameErrorResult =
//				result.Errors.Where(current => current == userNameErrorMessage).SingleOrDefault();

//			var emailErrorResult =
//				result.Errors.Where(current => current == emailErrorMessage).SingleOrDefault();

//			var idErrorResult =
//				result.Errors.Where(current => current == idErrorMessage).SingleOrDefault();

//			//Assert
//			Assert.Equal(expected: userNameErrorMessage, actual: userNameErrorResult);
//			Assert.Equal(expected: emailErrorMessage, actual: emailErrorResult);
//			Assert.Equal(expected: idErrorMessage, actual: idErrorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithInvalidEmailStructure()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var createdUser =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Id = createdUser.Id,
//				UserName = createdUser.UserName,
//				Email = "Test"
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.InvalidEmailStructure);

//			//Act
//			var result2 =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var errorResult =
//				result2.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithInvalidRoleId()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var createdUser =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Id = createdUser.Id,
//				UserName = createdUser.UserName,
//				Email = "Test",
//				RoleId = 5
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.RoleNotFound);

//			//Act
//			var result2 =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var errorResult =
//				result2.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithExistingEmail()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var createdUser1 =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
//			{
//				UserName = userName + "Test",
//				Password = password,
//				Email = $"Test1{random1}@gmail.com"
//			};

//			var result2 =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

//			var createdUser2 =
//				await UserServices.GetByUserNameAsync(userName: registerRequestViewModel2.UserName);

//			//Arrange
//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Id = createdUser1.Id,
//				UserName = createdUser1.UserName,
//				Email = createdUser2.Email
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.EmailExist);

//			//Act
//			var result3 =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var errorResult =
//				result3.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithExistingUserName()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var createdUser1 =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			RegisterRequestViewModel registerRequestViewModel2 = new RegisterRequestViewModel()
//			{
//				UserName = userName + "Test",
//				Password = password,
//				Email = $"Test1{random1}@gmail.com"
//			};

//			var result2 =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

//			var createdUser2 =
//				await UserServices.GetByUserNameAsync(userName: registerRequestViewModel2.UserName);

//			//Arrange
//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Id = createdUser1.Id,
//				UserName = createdUser2.UserName,
//				Email = createdUser1.Email
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.UserNameExist);

//			//Act
//			var result3 =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var errorResult =
//				result3.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithInvalidValues()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			var registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var registerRequestViewModel2 = new RegisterRequestViewModel()
//			{
//				UserName = userName + "Test",
//				Password = password,
//				Email = $"Test1{random1}@gmail.com"
//			};

//			var result2 =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel2);

//			var createdUser =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			var createdUser2 =
//				await UserServices.GetByUserNameAsync(userName: registerRequestViewModel2.UserName);

//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Id = createdUser.Id,
//				Email = "Test",
//				RoleId = -1,
//				UserName = createdUser2.UserName
//			};

//			string emailErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.InvalidEmailStructure);

//			string roleErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.RoleNotFound);

//			string userErrorMessage = string.Format
//				(Resources.Messages.ErrorMessages.UserNameExist);

//			//Act
//			var result3 =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var userNameErrorResult =
//				result3.Errors.Where(current => current == userErrorMessage).SingleOrDefault();

//			var emailErrorResult =
//				result3.Errors.Where(current => current == emailErrorMessage).SingleOrDefault();

//			var roleErrorResult =
//				result3.Errors.Where(current => current == roleErrorMessage).SingleOrDefault();

//			//Assert
//			Assert.Equal(expected: userErrorMessage, actual: userNameErrorResult);
//			Assert.Equal(expected: emailErrorMessage, actual: emailErrorResult);
//			Assert.Equal(expected: roleErrorMessage, actual: roleErrorResult);
//		}

//		[Fact]
//		public async Task TestUpdateUserWithCorrectValues()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var createdUser =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			var updateUserRequestViewModel = new UpdateUserByAdminRequestViewModel()
//			{
//				Id = createdUser.Id,
//				UserName = createdUser.UserName,
//				Email = $"Test1{random1}@gmail.com",
//				RoleId = createdUser.RoleId,
//				IsDeleted = true
//			};

//			string successMessage = string.Format
//				(Resources.Messages.SuccessMessages.UpdateSuccessful);

//			//Act
//			var result3 =
//				await UserServices.UpdateUserByAdminAsync(updateUserRequestViewModel: updateUserRequestViewModel);

//			var users =
//				await UserServices.UnitOfWork.UserRepository.GetAllAsync();

//			var updatedUser =
//				users.Where(current => current.Id == updateUserRequestViewModel.Id &&
//				current.UserName == updateUserRequestViewModel.UserName &&
//				current.Email == updateUserRequestViewModel.Email &&
//				current.IsDeleted == updateUserRequestViewModel.IsDeleted);

//			var successResult =
//				result3.Successes.Where(current => current == successMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: successMessage, actual: successResult);
//			Assert.NotNull(updatedUser);
//		}
//		#endregion /UpdateUserAsync

//		#region DeleteUserAsync
//		[Fact]
//		public async Task TestDeleteUserWithNullId()
//		{
//			//Arrange
//			var deleteUserRequestViewModel = new DeleteUserRequestViewModel()
//			{
//				Id = null
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
//				nameof(deleteUserRequestViewModel.Id), nameof(deleteUserRequestViewModel));

//			//Act
//			var result =
//				await UserServices.DeleteUserAsync(deleteUserRequestViewModel: deleteUserRequestViewModel);

//			var errorResult =
//				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestDeleteUserWithIncorrectId()
//		{
//			//Arrange
//			var deleteUserRequestViewModel = new DeleteUserRequestViewModel()
//			{
//				Id = Guid.NewGuid()
//			};

//			string errorMessage = string.Format
//				(Resources.Messages.ErrorMessages.UserNotFound);

//			//Act
//			var result =
//				await UserServices.DeleteUserAsync(deleteUserRequestViewModel: deleteUserRequestViewModel);

//			var errorResult =
//				result.Errors.Where(current => current == errorMessage).FirstOrDefault();

//			//Assert
//			Assert.Equal(expected: errorMessage, actual: errorResult);
//		}

//		[Fact]
//		public async Task TestDeleteUserWithCorrectId()
//		{
//			//Arrange
//			string userName = Guid.NewGuid().ToString();

//			string random = userName.Replace("-", "");

//			string random1 = random.Remove(0, 15);

//			string password = Guid.NewGuid().ToString();

//			RegisterRequestViewModel registerRequestViewModel = new RegisterRequestViewModel()
//			{
//				UserName = userName,
//				Password = password,
//				Email = $"Test{random1}@gmail.com"
//			};

//			var result =
//				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

//			var createdUser =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			var deleteUserRequestViewModel = new DeleteUserRequestViewModel()
//			{
//				Id = createdUser.Id
//			};

//			string successMessage = string.Format
//				(Resources.Messages.SuccessMessages.DeleteUserSuccessful);

//			//Act
//			var result2 =
//				await UserServices.DeleteUserAsync(deleteUserRequestViewModel: deleteUserRequestViewModel);

//			var errorResult =
//				result2.Successes.Where(current => current == successMessage).FirstOrDefault();

//			var deletedUser =
//				await UserServices.GetByUserNameAsync(userName: userName);

//			var isUserDeleted = deletedUser.IsDeleted == true;

//			//Assert
//			Assert.Equal(expected: successMessage, actual: errorResult);
//			Assert.True(isUserDeleted);
//		}
//		#endregion /DeleteUserAsync
//	}
//}
