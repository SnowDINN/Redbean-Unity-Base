using System;
using System.Threading.Tasks;

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
		
		public Task<bool> Initialize()
		{
			throw new NotImplementedException();
		}

		public Task<AuthenticationResult> Login()
		{
			throw new NotImplementedException();
		}

		public Task<AuthenticationResult> AutoLogin()
		{
			throw new NotImplementedException();
		}
	}
}