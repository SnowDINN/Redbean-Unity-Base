using System.Threading.Tasks;
using System.Web;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetAccessTokenAndUserProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetAccessTokenAndUserRequest(HttpUtility.UrlEncode($"{args[0]}".Encryption()));
			if (request.ErrorCode > 0)
				return request.Response;
			
			ApiContainer.SetAccessToken(new TokenResponse
			{
				AccessToken = request.Response.AccessToken,
				RefreshToken = request.Response.RefreshToken,
				AccessTokenExpire = request.Response.AccessTokenExpire,
				RefreshTokenExpire = request.Response.RefreshTokenExpire
			});
			var user = new UserModel(request.Response).ModelPublish(true);

			await AppBootstrap.BootstrapSetup(AppBootstrapType.SignInUser);
			
			Log.Print($"Login user's data. [ {user.Information.Nickname} | {user.Social.Id} ]");
			return request.Response;
		}
	}
}