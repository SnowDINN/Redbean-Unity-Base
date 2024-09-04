using System.Threading;
using System.Threading.Tasks;
using Firebase.Messaging;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class PostAccessTokenAndUserProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var parameter = args[0] as AuthenticationRequest;
			parameter.id = parameter.id.Encryption();
			
			var response = await ApiPostRequest.PostAccessTokenAndUserRequest(parameter, cancellationToken);
			if (!response.isSuccess)
				return response;
			
			ApiAuthentication.SetAccessToken(new TokenResponse
			{
				AccessToken = response.Response.Token.AccessToken,
				RefreshToken = response.Response.Token.RefreshToken,
				AccessTokenExpire = response.Response.Token.AccessTokenExpire,
				RefreshTokenExpire = response.Response.Token.RefreshTokenExpire
			});
			var user = this.GetModel<UserModel>();
			user.Information = response.Response.User.Information;
			user.Social = response.Response.User.Social;
			user.Log = response.Response.User.Log;
			user.Override();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.Login);
			await FirebaseMessaging.SubscribeAsync(user.Information.Id);
			
			Log.Print($"Login user's data. [ {user.Information.Id} | {user.Information.Nickname} ]");
			return response;
		}
	}
}