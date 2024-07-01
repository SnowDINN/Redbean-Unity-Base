using System.Threading.Tasks;
using System.Web;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetUserProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetUserRequest(HttpUtility.UrlEncode($"{args[0]}".Encryption()));
			if (request.ErrorCode > 0)
				return request.Response;
			
			ApiContainer.SetAccessToken(new TokenResponse
			{
				AccessToken = request.Response.AccessToken,
				RefreshToken = request.Response.RefreshToken,
				AccessTokenExpire = request.Response.AccessTokenExpire,
				RefreshTokenExpire = request.Response.RefreshTokenExpire
			});

			var user = this.GetModel<UserModel>();
			user.Response = new UserResponse
			{
				Social = request.Response.Social,
				Information = request.Response.Information
			};
			user.ModelPublish().SetPlayerPrefs();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.SignInUser);
			
			Log.Print($"Login user's data. [ {user.Response.Information.Nickname} | {user.Response.Social.Id} ]");
			return request.Response;
		}
	}
}