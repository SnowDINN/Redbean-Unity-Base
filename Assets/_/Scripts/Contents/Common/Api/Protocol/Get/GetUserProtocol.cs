using System.Threading.Tasks;
using System.Web;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetUserProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			var response = await ApiGetRequest.GetUserRequest(HttpUtility.UrlEncode($"{args[0]}".Encryption()));
			if (response.ErrorCode > 0)
				return response.Response;
			
			ApiContainer.SetAccessToken(new TokenResponse
			{
				AccessToken = response.Response.AccessToken,
				RefreshToken = response.Response.RefreshToken,
				AccessTokenExpire = response.Response.AccessTokenExpire,
				RefreshTokenExpire = response.Response.RefreshTokenExpire
			});

			var user = this.GetModel<UserModel>();
			user.Response = new UserResponse
			{
				Social = response.Response.Social,
				Information = response.Response.Information
			};
			user.ModelPublish().SetPlayerPrefs();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.SignInUser);
			
			Log.Print($"Login user's data. [ {user.Response.Information.Nickname} | {user.Response.Social.Id} ]");
			return response.Response;
		}
	}
}