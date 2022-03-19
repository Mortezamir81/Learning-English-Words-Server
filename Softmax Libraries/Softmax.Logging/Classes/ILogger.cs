using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Dtat.Logging
{
	public interface ILogger<T> where T : class
	{
		Task<bool> LogTrace
			(string message, [CallerMemberName] string methodName = null,
				System.Collections.Hashtable parameters = null);

		Task<bool> LogDebug
			(string message, [CallerMemberName] string methodName = null,
				System.Collections.Hashtable parameters = null);

		Task<bool> LogInformation
			(string message, [CallerMemberName] string methodName = null,
				System.Collections.Hashtable parameters = null);

		Task<bool> LogWarning
			(string message, [CallerMemberName] string methodName = null,
				System.Collections.Hashtable parameters = null);

		Task<bool> LogError
			(System.Exception exception,
			string message = null, [CallerMemberName] string methodName = null,
				System.Collections.Hashtable parameters = null);

		Task<bool> LogCritical
			(System.Exception exception,
			string message = null, [CallerMemberName] string methodName = null,
				System.Collections.Hashtable parameters = null);
	}
}
