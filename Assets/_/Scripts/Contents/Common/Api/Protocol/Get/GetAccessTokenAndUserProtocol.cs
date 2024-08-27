using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Firebase.Auth;
using Firebase.Messaging;
using Redbean.Auth;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetAccessTokenAndUserProtocol : ApiProtocol
	{
		protected override async Task<object> Request(CancellationToken cancellationToken = default)
		{
			var credential = (args[0] as AuthenticationResult).Credential;
			var firebaseUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
			
			var request = await ApiGetRequest.GetAccessTokenAndUserRequest
				(new[] { HttpUtility.UrlEncode($"{firebaseUser.UserId}".Encryption()) }, cancellationToken);
			
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