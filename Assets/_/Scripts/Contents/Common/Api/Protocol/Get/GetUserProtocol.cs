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
				return response;
			
			ApiContainer.SetAccessToken(new TokenResponse
			{
				AccessToken = response.AccessToken,
				RefreshToken = response.RefreshToken,
				AccessTokenExpire = response.AccessTokenExpire,
				RefreshTokenExpire = response.RefreshTokenExpire
			});

			var user = this.GetModel<UserModel>();
			user.Response = new UserResponse
			{
				Social = response.Social,
				Information = response.Information
			};
			user.ModelPublish().SetPlayerPrefs();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.SignInUser);
			
			Log.Print($"Login user's data. [ {user.Response.Information.Nickname} | {user.Response.Social.Id} ]");
			return response;
		}
	}
}