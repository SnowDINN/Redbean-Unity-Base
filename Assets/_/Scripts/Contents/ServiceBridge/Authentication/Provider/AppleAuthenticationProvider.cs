using Cysharp.Threading.Tasks;

namespace Redbean.ServiceBridge
{
	public enum AppleAuthErrorCode
	{
		Success = 0,
		GoogleLoginCancelled = 11000000
	}
	
	public class AppleAuthenticationProvider : IAuthentication
	{
		public AuthenticationType Type => AuthenticationType.Apple;
		
		public UniTask<bool> Initialize()
		{
			throw new System.NotImplementedException();
		}

		public UniTask<AuthenticationResult> Login()
		{
			throw new System.NotImplementedException();
		}

		public UniTask<AuthenticationResult> AutoLogin()
		{
			throw new System.NotImplementedException();
		}
	}
}