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
			if (IsInitialize)
				Task.FromResult(IsInitialize);
			
			IsInitialize = true;
			return Task.FromResult(true);
		}

		public Task<AuthenticationResult> Login(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<AuthenticationResult>();
			var result = new AuthenticationResult();
			
			var user = string.IsNullOrEmpty(Database.Load<string>(PlayerPrefsKey.GUEST_USER_ID))
				? new UserModel
				{
					Database =
					{
						Information =
						{
							Id = "Guest"
						}
					}
				}
				: new UserModel
				{
					Database =
					{
						Information =
						{
							Id = Database.Load<string>(PlayerPrefsKey.GUEST_USER_ID)
						}
					}
				};
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
			
			var user = new UserModel
			{
				Database =
				{
					Information =
					{
						Id = Database.Load<string>(PlayerPrefsKey.GUEST_USER_ID)
					}
				}
			};
			user.Override();

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