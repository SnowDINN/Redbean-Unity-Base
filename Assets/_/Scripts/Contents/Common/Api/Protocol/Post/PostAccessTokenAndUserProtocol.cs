using System.Threading;
using System.Threading.Tasks;
using Firebase.Messaging;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class PostAccessTokenAndUserProtocol : ApiProtocol
	{
		protected override async Task<IApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var parameter = args[0] as AuthenticationRequest;
			parameter.id = parameter.id.Encryption();
			
			var response = await ApiPostRequest.PostAccessTokenAndUserRequest(parameter, cancellationToken);
			if (response.ErrorCode != 0)
				return response;
			
			ApiAuthentication.SetAccessToken(new TokenResponse
			{
				AccessToken = response.Response.Token.AccessToken,
				RefreshToken = response.Response.Token.RefreshToken,
				AccessTokenExpire = response.Response.Token.AccessTokenExpire,
				RefreshTokenExpire = response.Response.Token.RefreshTokenExpire
			});
			var user = new UserModel(response.Response).Override();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.Login);
			await FirebaseMessaging.SubscribeAsync(user.Information.Id);
			
			Log.Print($"Login user's data. [ {user.Information.Id} | {user.Information.Nickname} ]");
			return response;
		}
	}
}