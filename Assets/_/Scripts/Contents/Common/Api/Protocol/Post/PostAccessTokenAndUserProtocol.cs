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
			if (!response.IsSuccess)
				return response;
			
			ApiAuthentication.SetAccessToken(new TokenResponse
			{
				AccessToken = response.Response.Token.AccessToken,
				RefreshToken = response.Response.Token.RefreshToken,
				AccessTokenExpire = response.Response.Token.AccessTokenExpire,
				RefreshTokenExpire = response.Response.Token.RefreshTokenExpire
			});
			
			var user = new UserModel
			{
				Database =
				{
					Information = response.Response.User.Information,
					Social = response.Response.User.Social,
					Log = response.Response.User.Log
				}
			};
			user.Override();

			await AppSettings.BootstrapSetup(BootstrapKey.OnLogin);
			await FirebaseMessaging.SubscribeAsync(user.Database.Information.Id);
			
			Log.Print($"Login user's data. [ {user.Database.Information.Id} | {user.Database.Information.Nickname} ]");
			return response;
		}
	}
}