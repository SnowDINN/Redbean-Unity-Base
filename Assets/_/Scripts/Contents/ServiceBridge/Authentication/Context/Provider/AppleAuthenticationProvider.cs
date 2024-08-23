using System;
using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Auth
{
	public enum AppleAuthErrorCode
	{
		Success = 0,
		GoogleLoginCancelled = 11000000
	}
	
	public class AppleAuthenticationProvider : IAuthentication
	{
		public AuthenticationType Type => AuthenticationType.Apple;
		
		public Task<bool> Initialize(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<AuthenticationResult> Login(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<AuthenticationResult> AutoLogin(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}