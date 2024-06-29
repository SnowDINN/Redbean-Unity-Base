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
				return response.Value;
			
			ApiContainer.SetAccessToken(new TokenResponse
			{
				AccessToken = response.Value.AccessToken,
				RefreshToken = response.Value.RefreshToken,
				AccessTokenExpire = response.Value.AccessTokenExpire,
				RefreshTokenExpire = response.Value.RefreshTokenExpire
			});

			var user = this.GetModel<UserModel>();
			user.Response = new UserResponse
			{
				Social = response.Value.Social,
				Information = response.Value.Information
			};
			user.ModelPublish().SetPlayerPrefs();

			await AppBootstrap.BootstrapSetup(AppBootstrapType.SignInUser);
			
			Log.Print($"Login user's data. [ {user.Response.Information.Nickname} | {user.Response.Social.Id} ]");
			return response.Value;
		}
	}
}