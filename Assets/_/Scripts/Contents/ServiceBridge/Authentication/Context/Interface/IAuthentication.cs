using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;

namespace Redbean.Auth
{
	public interface IAuthentication
	{
		AuthenticationType Type { get; }
		Task<bool> Initialize(CancellationToken cancellationToken = default);
		Task<AuthenticationResult> Login(CancellationToken cancellationToken = default);
		Task<AuthenticationResult> AutoLogin(CancellationToken cancellationToken = default);
	}

	public class AuthenticationResult
	{
		public Credential Credential;
		public int Code;
		public string Message;
	}
}