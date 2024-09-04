using System.Threading;
using System.Threading.Tasks;
using Redbean.Api;
using Redbean.MVP.Content;
using Redbean.Utility;

namespace Redbean.Auth
{
	public class GuestAuthenticationProvider : IAuthentication
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

			var user = this.GetModel<UserModel>();
			user.Information.Id = "Guest";
			user.Override();
			
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

			var user = LocalDatabase.Load<UserModel>(PlayerPrefsKey.GUEST_USER_ID);
			user?.Override();

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