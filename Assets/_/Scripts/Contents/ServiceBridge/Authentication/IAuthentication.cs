using Cysharp.Threading.Tasks;
using Firebase.Auth;

namespace Redbean.ServiceBridge
{
	public interface IAuthentication
	{
		AuthenticationType Type { get; }
		UniTask<bool> Initialize();
		UniTask<AuthenticationResult> Login();
		UniTask<AuthenticationResult> AutoLogin();
	}

	public class AuthenticationResult
	{
		public Credential Credential;
		public int Code;
		public string Message;
	}
}