using System.Threading;
using System.Threading.Tasks;
using Redbean.MVP.Content;

namespace Redbean.Auth
{
	public class GuestAuthenticationProvider : IAuthenticationContainer
	{
		public AuthenticationType Type => AuthenticationType.Guest;
		
		public bool IsInitialize { get; set; }
		
		public Task<bool> Initialize(CancellationToken cancellationToken = default)
		{
			return Task.FromResult(true);
		}

		public Task<AuthenticationResult> Login(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<AuthenticationResult>();
			var result = new AuthenticationResult();

			var user = new UserModel
			{
				Social =
				{
					Platform = "Guest"
				}
			}.Override();
			
			result = new AuthenticationResult
			{
				Code = (int)AppleAuthErrorCode.Success,
				Credential = null
			};
			completionSource.SetResult(result);

			return completionSource.Task;
		}

		public Task<AuthenticationResult> AutoLogin(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<AuthenticationResult>();
			var result = new AuthenticationResult();
			
			result = new AuthenticationResult
			{
				Code = (int)AppleAuthErrorCode.Success,
				Credential = null
			};
			completionSource.SetResult(result);
			
			return completionSource.Task;
		}
	}
}