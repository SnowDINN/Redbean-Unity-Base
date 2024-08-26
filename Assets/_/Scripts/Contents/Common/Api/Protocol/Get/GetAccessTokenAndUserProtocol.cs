using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Firebase.Messaging;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetAccessTokenAndUserProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			var request = await ApiGetRequest.GetAccessTokenAndUserRequest
				(new[] { HttpUtility.UrlEncode($"{args[0]}".Encryption()) }, cancellationToken);
			
			if (request.ErrorCode != 0)
				return request.Response;
			
			ApiAuthentication.SetAccessToken(new TokenResponse
			{
				AccessToken = request.Response.Token.AccessToken,
				RefreshToken = request.Response.Token.RefreshToken,
				AccessTokenExpire = request.Response.Token.AccessTokenExpire,
				RefreshTokenExpire = request.Response.Token.RefreshTokenExpire
			});
			var user = new UserModel(request.Response).ModelPublish();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.Login);
			await FirebaseMessaging.SubscribeAsync(user.Information.Id);
			
			Log.Print($"Login user's data. [ {user.Information.Id} | {user.Information.Nickname} ]");
			return request.Response;
		}
	}
}