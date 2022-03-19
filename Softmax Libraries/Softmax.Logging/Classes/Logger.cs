using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Dtat.Logging
{
	public abstract class Logger<T> : object, ILogger<T> where T : class
	{
		#region Constructor
		protected Logger(IHttpContextAccessor httpContextAccessor = null) : base()
		{
			HttpContextAccessor = httpContextAccessor;
		}
		#endregion /Constructor

		public IHttpContextAccessor HttpContextAccessor { get; }

		#region GetExceptions
		protected virtual string GetExceptions(Exception exception)
		{
			if (exception == null)
			{
				return null;
			}

			var stringBuilder =
				new StringBuilder();

			int index = 0;
			Exception currentException = exception;

			while (currentException != null)
			{
				if (index == 0)
				{
					stringBuilder.Append($"<{ nameof(Exception) }>");
				}
				else
				{
					stringBuilder.Append($"<{ nameof(Exception.InnerException) }>");
				}

				stringBuilder.Append($"{currentException.Message}");

				if (index == 0)
				{
					stringBuilder.Append($"</{ nameof(Exception) }>");
				}
				else
				{
					stringBuilder.Append($"</{ nameof(Exception.InnerException) }>");
				}

				index++;

				currentException =
					currentException.InnerException;
			}

			string result =
				stringBuilder.ToString();

			return result;
		}
		#endregion /GetExceptions

		#region GetParameters
		protected virtual string GetParameters(Hashtable parameters)
		{
			if ((parameters == null) || (parameters.Count == 0))
			{
				return null;
			}

			var stringBuilder = new StringBuilder();

			foreach (DictionaryEntry item in parameters)
			{
				if (item.Key != null)
				{
					stringBuilder.Append("<parameter>");

					stringBuilder.Append($"<key>{ item.Key }</key>");

					if (item.Value == null)
					{
						stringBuilder.Append($"<value>NULL</value>");
					}
					else
					{
						stringBuilder.Append($"<value>{ item.Value }</value>");
					}

					stringBuilder.Append("</parameter>");
				}
			}

			string result = stringBuilder.ToString();

			return result;
		}
		#endregion /GetParameters

		#region Log
		protected async Task<bool>
			Log(LogLevel level,
				string message,
				Exception exception = null,
				Hashtable parameters = null,
				string methodName = null)
		{
			try
			{
				// **************************************************
				string currentCultureName =
					Thread.CurrentThread.CurrentCulture.Name;

				var newCultureInfo =
					new CultureInfo(name: "en-US");

				var currentCultureInfo =
					new CultureInfo(currentCultureName);

				Thread.CurrentThread.CurrentCulture = newCultureInfo;
				// **************************************************

				var logModel = new LogModel
				{
					Level = level,
				};

				await Task.Run(() =>
				{

					if ((HttpContextAccessor != null) &&
						(HttpContextAccessor.HttpContext != null) &&
						(HttpContextAccessor.HttpContext.Connection != null) &&
						(HttpContextAccessor.HttpContext.Connection.RemoteIpAddress != null))
					{
						logModel.RemoteIP =
							HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
					}

					if ((HttpContextAccessor != null) &&
						(HttpContextAccessor.HttpContext != null) &&
						(HttpContextAccessor.HttpContext.Connection != null) &&
						(HttpContextAccessor.HttpContext.Connection.LocalIpAddress != null))
					{
						logModel.LocalIP =
							HttpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
					}

					if ((HttpContextAccessor != null) &&
						(HttpContextAccessor.HttpContext != null) &&
						(HttpContextAccessor.HttpContext.Connection != null))
					{
						logModel.LocalPort =
							HttpContextAccessor.HttpContext.Connection.LocalPort.ToString();
					}

					if ((HttpContextAccessor != null) &&
						(HttpContextAccessor.HttpContext != null) &&
						(HttpContextAccessor.HttpContext.User != null) &&
						(HttpContextAccessor.HttpContext.User.Identity != null))
					{
						logModel.Username =
							HttpContextAccessor.HttpContext.User.Identity.Name;
					}

					if ((HttpContextAccessor != null) &&
						(HttpContextAccessor.HttpContext != null) &&
						(HttpContextAccessor.HttpContext.Request != null))
					{
						logModel.RequestPath =
							HttpContextAccessor.HttpContext.Request.Path;

						logModel.HttpReferrer =
							HttpContextAccessor.HttpContext.Request.Headers["Referer"];
					}

					logModel.ApplicationName =
						typeof(T).GetTypeInfo().Assembly.FullName.ToString();

					logModel.ClassName = typeof(T).Name;

					if (!string.IsNullOrWhiteSpace(methodName))
						logModel.MethodName = methodName;

					logModel.Namespace = typeof(T).Namespace;

					logModel.Message = message;

					logModel.Exceptions =
						GetExceptions(exception: exception);

					logModel.Parameters =
						GetParameters(parameters: parameters);



					LogByFavoriteLibrary(logModel: logModel, exception: exception);



					// **************************************************
					Thread.CurrentThread.CurrentCulture = currentCultureInfo;
					// **************************************************
				});

				return true;
			}
			catch
			{
				return false;
			}
		}
		#endregion /Log

		protected abstract void LogByFavoriteLibrary(LogModel logModel, Exception exception);

		#region LogTrace
		public async virtual Task<bool> LogTrace
			(string message,
			[CallerMemberName] string methodName = null,
			Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			bool result =
				await
					Log(methodName: methodName,
						level: LogLevel.Trace,
						message: message,
						exception: null,
						parameters: parameters);

			return result;
		}
		#endregion /LogTrace

		#region LogDebug
		public async virtual Task<bool> LogDebug
			(string message,
			[CallerMemberName] string methodName = null,
			Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			bool result =
				await
					Log(methodName: methodName,
					level: LogLevel.Debug,
					message: message,
					exception: null,
					parameters: parameters);

			return result;
		}
		#endregion /LogDebug

		#region LogInformation
		public async virtual Task<bool> LogInformation
			(string message,
			[CallerMemberName] string methodName = null,
			Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			bool result =
				await
					Log(methodName: methodName,
					level: LogLevel.Information,
					message: message,
					exception: null,
					parameters: parameters);

			return result;
		}
		#endregion /LogInformation

		#region LogWarning
		public async virtual Task<bool> LogWarning
			(string message,
			[CallerMemberName] string methodName = null,
			Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			bool result =
				await
					Log(methodName: methodName,
					level: LogLevel.Warning,
					message: message,
					exception: null,
					parameters: parameters);

			return result;
		}
		#endregion /LogWarning

		#region LogError
		public async virtual Task<bool> LogError
			(Exception exception,
			string message = null,
			[CallerMemberName] string methodName = null,
			Hashtable parameters = null)
		{
			if (exception == null)
			{
				return false;
			}

			bool result =
				await
					Log(methodName: methodName,
					level: LogLevel.Error,
					message: message,
					exception: exception,
					parameters: parameters);

			return result;
		}
		#endregion /LogError

		#region LogCritical
		public async virtual Task<bool> LogCritical
			(Exception exception,
			string message = null,
			[CallerMemberName] string methodName = null,
			Hashtable parameters = null)
		{
			if (exception == null)
			{
				return false;
			}

			bool result =
				await
					Log(methodName: methodName,
					level: LogLevel.Critical,
					message: message,
					exception: exception,
					parameters: parameters);

			return result;
		}
		#endregion /LogCritical
	}
}
