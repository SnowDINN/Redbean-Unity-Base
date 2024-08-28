using System.Threading;
using System.Threading.Tasks;
using Firebase.Messaging;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class PostAccessTokenAndUserProtocol : ApiProtocol
	{
		protected override async Task<object> Request(CancellationToken cancellationToken = default)
		{
			var parameter = args[0] as AuthenticationRequest;
			parameter.id = parameter.id.Encryption();
			
			var request = await ApiPostRequest.PostAccessTokenAndUserRequest(parameter, cancellationToken);
			if (request.ErrorCode != 0)
				return request.Response;
			
			ApiAuthentication.SetAccessToken(new TokenResponse
			{
				AccessToken = request.Response.Token.AccessToken,
				RefreshToken = request.Response.Token.RefreshToken,
				AccessTokenExpire = request.Response.Token.AccessTokenExpire,
				RefreshTokenExpire = request.Response.Token.RefreshTokenExpire
			});
			var user = new UserModel(request.Response).Override();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.Login);
			await FirebaseMessaging.SubscribeAsync(user.Information.Id);
			
			Log.Print($"Login user's data. [ {user.Information.Id} | {user.Information.Nickname} ]");
			return request.Response;
		}
	}
}